using HealthuU.BLL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces.Encryption
{
    public interface IHybridEncryptionService
    {
        HybridPayload Encrypt(string plainText);
        string Decrypt(HybridPayload payload);
    }

}
