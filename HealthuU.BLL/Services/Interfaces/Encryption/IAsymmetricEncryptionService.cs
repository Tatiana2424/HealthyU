using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces.Encryption
{
    public interface IAsymmetricEncryptionService
    {
        string EncryptWithPublicKey(string plainText);
        string DecryptWithPrivateKey(string cipherText);
        string SignData(string plainText);
        bool VerifySignature(string plainText, string signature);
    }

}
