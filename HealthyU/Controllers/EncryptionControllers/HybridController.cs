//using HealthuU.BLL.Model;
//using HealthuU.BLL.Services.Interfaces.Encryption;
//using HealthyU.Controllers.BaseController;
//using Microsoft.AspNetCore.Mvc;

//namespace HealthyU.WebApi.Controllers.EncryptionControllers
//{
//    public class HybridController: BaseApiController
//    {
//        private readonly IHybridEncryptionService _hybridEncryptionService;

//        public HybridController(IHybridEncryptionService hybridEncryptionService)
//        {
//            _hybridEncryptionService = hybridEncryptionService;
//        }

//        [HttpPost]
//        public ActionResult<HybridPayload> Encrypt([FromBody] string text)
//        => _hybridEncryptionService.Encrypt(text);

//        [HttpPost]
//        public ActionResult<string> Decrypt([FromBody] HybridPayload payload)
//            => _hybridEncryptionService.Decrypt(payload);
//    }
//}
