using System.Security.Cryptography;


namespace HealthuU.BLL.Services.Interfaces.Encryption
{
    public interface IRsaKeyProvider
    {
        RSA GetPublicRsa();
        RSA GetPrivateRsa();
        void GenerateAndSaveKeyPair();
    }
}
