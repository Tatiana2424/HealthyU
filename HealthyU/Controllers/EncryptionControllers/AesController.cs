//using HealthuU.BLL.Services.Interfaces.Encryption;
//using HealthyU.Controllers.BaseController;
//using Microsoft.AspNetCore.Mvc;

//namespace HealthyU.WebApi.Controllers.EncryptionControllers
//{
//    public class AesController : BaseApiController
//    {
//        private readonly IAesEncryptionService _aes;

//        public AesController(IAesEncryptionService aes)
//        {
//            _aes = aes;
//        }

//        [HttpPost]
//        public IActionResult Encrypt(string plainText)
//        {
//            var encrypted = _aes.Encrypt(plainText);
//            return Ok(encrypted);
//        }

//        [HttpPost]
//        public IActionResult Decrypt(string encryptedText)
//        {
//            var decrypted = _aes.Decrypt(encryptedText);
//            return Ok(decrypted);
//        }
//    }
//}
