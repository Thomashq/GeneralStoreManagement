using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Service
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText, string base64Key, string base64Iv);
        string Decrypt(string cipherText, string base64Key, string base64Iv);
    }
}
