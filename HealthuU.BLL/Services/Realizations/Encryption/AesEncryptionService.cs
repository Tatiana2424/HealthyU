//using HealthuU.BLL.Model;
//using HealthuU.BLL.Services.Interfaces.Encryption;
//using Microsoft.Extensions.Options;
//using System.Security.Cryptography;
//using System.Text;

//namespace HealthuU.BLL.Services.Realizations.Encryption
//{
//    public class AesEncryptionService : IAesEncryptionService
//    {
//        private readonly byte[] _key;
//        private readonly byte[] _iv;

//        public AesEncryptionService(IOptions<AesSettings> settings)
//        {
//            _key = Encoding.UTF8.GetBytes(settings.Value.Key);
//            _iv = Encoding.UTF8.GetBytes(settings.Value.IV);
//        }

//        public string Encrypt(string plainText)
//        {
//            using var aes = Aes.Create();
//            aes.Key = _key;
//            aes.IV = _iv;

//            using var encryptor = aes.CreateEncryptor();
//            var plainBytes = Encoding.UTF8.GetBytes(plainText);
//            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

//            return Convert.ToBase64String(encryptedBytes);
//        }

//        public string Decrypt(string encryptedText)
//        {
//            using var aes = Aes.Create();
//            aes.Key = _key;
//            aes.IV = _iv;

//            using var decryptor = aes.CreateDecryptor();
//            var encryptedBytes = Convert.FromBase64String(encryptedText);
//            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

//            return Encoding.UTF8.GetString(decryptedBytes);
//        }
//    }
//}
