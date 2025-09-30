//using HealthuU.BLL.Model;
//using HealthuU.BLL.Services.Interfaces.Encryption;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//namespace HealthuU.BLL.Services.Realizations.Encryption
//{
//    public class RsaKeyProvider : IRsaKeyProvider
//    {
//        private readonly RsaSettings _settings;

//        public RsaKeyProvider(IOptions<RsaSettings> opts)
//        {
//            _settings = opts.Value;
//        }

//        public void GenerateAndSaveKeyPair()
//        {
//            using var rsa = RSA.Create(2048);
//            Directory.CreateDirectory(Path.GetDirectoryName(_settings.PublicKeyPath)!);

//            File.WriteAllText(_settings.PublicKeyPath, rsa.ToXmlString(false));
//            File.WriteAllText(_settings.PrivateKeyPath, rsa.ToXmlString(true));
//        }

//        public RSA GetPublicRsa()
//        {
//            var rsa = RSA.Create();
//            var xml = File.ReadAllText(_settings.PublicKeyPath);
//            rsa.FromXmlString(xml);
//            return rsa;
//        }

//        public RSA GetPrivateRsa()
//        {
//            var rsa = RSA.Create();
//            var xml = File.ReadAllText(_settings.PrivateKeyPath);
//            rsa.FromXmlString(xml);
//            return rsa;
//        }
//    }
//}
