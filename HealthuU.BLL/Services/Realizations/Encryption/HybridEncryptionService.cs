using HealthuU.BLL.Model;
using HealthuU.BLL.Services.Interfaces.Encryption;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Realizations.Encryption
{
    public class HybridEncryptionService : IHybridEncryptionService
    {
        private readonly byte[] _aesKey;
        private readonly byte[] _aesIv;
        private readonly IRsaKeyProvider _rsaKeys;

        public HybridEncryptionService(
            IOptions<AesSettings> aesOpts,
            IRsaKeyProvider rsaKeys)
        {
            var cfg = aesOpts.Value;
            _aesKey = Encoding.UTF8.GetBytes(cfg.Key);
            _aesIv = Encoding.UTF8.GetBytes(cfg.IV);
            _rsaKeys = rsaKeys;
        }

        public HybridPayload Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _aesKey;
            aes.IV = _aesIv;

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var keyIv = new byte[_aesKey.Length + _aesIv.Length];
            Buffer.BlockCopy(_aesKey, 0, keyIv, 0, _aesKey.Length);
            Buffer.BlockCopy(_aesIv, 0, keyIv, _aesKey.Length, _aesIv.Length);

            using var rsaPub = _rsaKeys.GetPublicRsa();
            var encryptedKeyBytes = rsaPub.Encrypt(keyIv, RSAEncryptionPadding.OaepSHA256);

            return new HybridPayload
            {
                EncryptedKey = Convert.ToBase64String(encryptedKeyBytes),
                EncryptedData = Convert.ToBase64String(cipherBytes)
            };
        }

        public string Decrypt(HybridPayload payload)
        {
            using var rsaPriv = _rsaKeys.GetPrivateRsa();
            var keyIv = rsaPriv.Decrypt(
                Convert.FromBase64String(payload.EncryptedKey),
                RSAEncryptionPadding.OaepSHA256
            );

            var key = new byte[_aesKey.Length];
            var iv = new byte[_aesIv.Length];
            Buffer.BlockCopy(keyIv, 0, key, 0, key.Length);
            Buffer.BlockCopy(keyIv, key.Length, iv, 0, iv.Length);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor();
            var cipherBytes = Convert.FromBase64String(payload.EncryptedData);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
