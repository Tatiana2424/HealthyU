using HealthuU.BLL.Services.Interfaces.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Realizations.Encryption
{
    public class RsaEncryptionService : IAsymmetricEncryptionService
    {
        private readonly IRsaKeyProvider _keys;

        public RsaEncryptionService(IRsaKeyProvider keys)
        {
            _keys = keys;
        }

        public string EncryptWithPublicKey(string plainText)
        {
            using var rsa = _keys.GetPublicRsa();
            var data = Encoding.UTF8.GetBytes(plainText);
            var cipher = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(cipher);
        }

        public string DecryptWithPrivateKey(string cipherText)
        {
            using var rsa = _keys.GetPrivateRsa();
            var data = Convert.FromBase64String(cipherText);
            var plain = rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(plain);
        }

        public string SignData(string plainText)
        {
            using var rsa = _keys.GetPrivateRsa();
            var data = Encoding.UTF8.GetBytes(plainText);
            var signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signature);
        }

        public bool VerifySignature(string plainText, string signature)
        {
            using var rsa = _keys.GetPublicRsa();
            var data = Encoding.UTF8.GetBytes(plainText);
            var sig = Convert.FromBase64String(signature);
            return rsa.VerifyData(data, sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
