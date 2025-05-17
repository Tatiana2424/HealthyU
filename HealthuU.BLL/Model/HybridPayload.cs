using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Model
{
    public class HybridPayload
    {
        public string EncryptedKey { get; set; } = string.Empty;
        public string EncryptedData { get; set; } = string.Empty;
    }
}
