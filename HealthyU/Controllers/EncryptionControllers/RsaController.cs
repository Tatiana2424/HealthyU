//using HealthuU.BLL.Model;
//using HealthuU.BLL.Services.Interfaces.Encryption;
//using HealthyU.Controllers.BaseController;
//using Microsoft.AspNetCore.Mvc;

//namespace HealthyU.WebApi.Controllers.EncryptionControllers
//{
//    public class RsaController : BaseApiController
//    {
//        private readonly IAsymmetricEncryptionService _rsa;
//        public RsaController(IAsymmetricEncryptionService rsa) => _rsa = rsa;

//        [HttpPost]
//        public IActionResult Encrypt([FromBody] string text)
//        {
//            return Ok(_rsa.EncryptWithPublicKey(text));
//        }

//        [HttpPost]
//        public IActionResult Decrypt([FromBody] string cipher)
//        {
//            return Ok(_rsa.DecryptWithPrivateKey(cipher));
//        }

//        [HttpPost]
//        public IActionResult Sign([FromBody] string text)
//        {
//            return Ok(_rsa.SignData(text));
//        }

//        [HttpPost]
//        public IActionResult Verify([FromBody] RsaVerifyRequest req)
//        {
//            return Ok(new { valid = _rsa.VerifySignature(req.Text, req.Signature) });
//        }
//    }
//}
