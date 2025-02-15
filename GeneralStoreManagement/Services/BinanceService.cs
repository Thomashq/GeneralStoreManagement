﻿using Domain.Interfaces.Service;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace GeneralStoreManagement.Services
{
    public class BinanceService : IDisposable
    {
        private readonly ConcurrentDictionary<string, ClientWebSocket> _webSocketConnections;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, object>> _currentData;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly HttpClient _httpClient;
        private const string BinanceWebSocketBaseUrl = "wss://stream.binance.com:9443/ws/";

        public BinanceService(string apiKey, string apiSecret)
        {
            _webSocketConnections = new ConcurrentDictionary<string, ClientWebSocket>();
            _currentData = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();
            _cancellationTokenSource = new CancellationTokenSource();
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.binance.com") };
        }
        public async Task<string> GetAccountInfoAsync()
        {
            var endpoint = "/api/v3/account";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var queryString = $"timestamp={timestamp}";

            var signature = GenerateSignature(queryString);
            var requestUrl = $"{endpoint}?{queryString}&signature={signature}";

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("X-MBX-APIKEY", _apiKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao acessar informações da conta: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        private string GenerateSignature(string queryString)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_apiSecret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(queryString));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public async Task ConnectAsync(string pair)
        {
            if (_webSocketConnections.ContainsKey(pair))
                throw new InvalidOperationException($"Already connected to stream for pair: {pair}");

            var clientWebSocket = new ClientWebSocket();
            var url = $"{BinanceWebSocketBaseUrl}{pair}@ticker";

            await clientWebSocket.ConnectAsync(new Uri(url), _cancellationTokenSource.Token);

            _webSocketConnections[pair] = clientWebSocket;

            _currentData[pair] = new ConcurrentDictionary<string, object>();

            _ = Task.Run(() => ListenToWebSocketAsync(pair, clientWebSocket, _cancellationTokenSource.Token));
            Console.WriteLine($"Connected to Binance WebSocket for pair: {pair}");
        }

        private async Task ListenToWebSocketAsync(string pair, ClientWebSocket clientWebSocket, CancellationToken cancellationToken)
        {
            var buffer = new byte[1024 * 4];

            while (clientWebSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        ProcessMessage(pair, message);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine($"WebSocket connection for pair {pair} closed.");
                        await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", cancellationToken);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while receiving message for pair {pair}: {ex.Message}");
                    break;
                }
            }
        }

        private void ProcessMessage(string pair, string message)
        {
            try
            {
                var jsonData = JsonSerializer.Deserialize<JsonElement>(message);

                var currentPrice = jsonData.GetProperty("c").GetString(); // Preço atual
                var priceChangePercent = jsonData.GetProperty("P").GetString(); // Porcentagem de mudança

                var pairData = _currentData[pair];
                pairData["price"] = currentPrice;
                pairData["percentageChange"] = priceChangePercent;

                Console.WriteLine($"Pair: {pair}, Price: {currentPrice}, Change: {priceChangePercent}%");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message for pair {pair}: {ex.Message}");
            }
        }

        public object GetCurrentData(string pair)
        {
            return _currentData.TryGetValue(pair, out var data) ? data : null;
        }

        public async Task DisconnectAsync(string pair)
        {
            if (_webSocketConnections.TryRemove(pair, out var clientWebSocket))
            {
                if (clientWebSocket.State == WebSocketState.Open)
                {
                    await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnecting", CancellationToken.None);
                    Console.WriteLine($"Disconnected from WebSocket for pair: {pair}");
                }
            }

            _currentData.TryRemove(pair, out _);
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();

            foreach (var clientWebSocket in _webSocketConnections.Values)
            {
                clientWebSocket.Dispose();
            }
        }
    }
}
