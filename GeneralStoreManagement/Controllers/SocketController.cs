using Domain.Interfaces;
using Domain.Shared;
using GeneralStoreManagement.Services;
using Microsoft.AspNetCore.Mvc;
namespace GeneralStoreManagement.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class SocketController : ControllerBase
    {
        private readonly BinanceService _webSocketService;

        public SocketController(BinanceService webSocketService)
        {
            _webSocketService = webSocketService;
        }

        [HttpPost("connect")]
        public async Task<IActionResult> Connect([FromQuery] string pair)
        {
            if (string.IsNullOrEmpty(pair))
            {
                return BadRequest(new ApiResponse<string>(
                    null,
                    false,
                    "Pair parameter is required.",
                    new List<string> { "Missing pair query parameter." }
                ));
            }

            try
            {
                await _webSocketService.ConnectAsync(pair);
                return Ok(new ApiResponse<string>(
                    null,
                    true,
                    $"Connected to Binance WebSocket for pair: {pair}."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(
                    null,
                    false,
                    "Error connecting to WebSocket.",
                    new List<string> { ex.Message }
                ));
            }
        }

        [HttpGet("data")]
        public IActionResult GetPairData([FromQuery] string pair)
        {
            if (string.IsNullOrEmpty(pair))
            {
                return BadRequest(new ApiResponse<string>(
                    null,
                    false,
                    "Pair parameter is required.",
                    new List<string> { "Missing pair query parameter." }
                ));
            }

            var data = _webSocketService.GetCurrentData(pair);
            if (data == null)
            {
                return NotFound(new ApiResponse<object>(
                    null,
                    false,
                    $"No data available for pair: {pair}."
                ));
            }

            return Ok(new ApiResponse<object>(
                data,
                true,
                $"Data retrieved successfully for pair: {pair}."
            ));
        }

        [HttpPost("disconnect")]
        public async Task<IActionResult> Disconnect([FromQuery] string pair)
        {
            if (string.IsNullOrEmpty(pair))
            {
                return BadRequest(new ApiResponse<string>(
                    null,
                    false,
                    "Pair parameter is required.",
                    new List<string> { "Missing pair query parameter." }
                ));
            }

            try
            {
                await _webSocketService.DisconnectAsync(pair);
                return Ok(new ApiResponse<string>(
                    null,
                    true,
                    $"Disconnected from Binance WebSocket for pair: {pair}."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(
                    null,
                    false,
                    "Error disconnecting from WebSocket.",
                    new List<string> { ex.Message }
                ));
            }
        }

    }
}
