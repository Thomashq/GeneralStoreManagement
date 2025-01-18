using Domain.Interfaces.Service;
using System;
using System.IO;
using System.Security.Cryptography;

public class EncryptionService : IEncryptionService 
{
    public string Encrypt(string plainText, string base64Key, string base64Iv)
    {
        var key = Convert.FromBase64String(base64Key);
        var iv = Convert.FromBase64String(base64Iv);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream();
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cryptoStream))
        {
            writer.Write(plainText);
        }

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public string Decrypt(string cipherText, string base64Key, string base64Iv)
    {
        var key = Convert.FromBase64String(base64Key);
        var iv = Convert.FromBase64String(base64Iv);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(Convert.FromBase64String(cipherText));
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream);

        return reader.ReadToEnd();
    }
}
