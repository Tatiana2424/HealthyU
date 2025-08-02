//using System.Threading.Tasks;
//using Xunit;
//using Moq;
//using HealthuU.BLL.Services.Realizations;
//using HealthuU.BLL.Services.Interfaces;
//using HealthuU.BLL.DTO;
//using HealthuU.BLL.Helpers;
//using HealthuU.BLL.Services.Interfaces.Logging;
//using HealthyU.DAL.Repositories.Interfaces;
//using Moq.Protected;
//using Newtonsoft.Json;
//using System.Net;
//using System.Text;
//using CSharpFunctionalExtensions;
//using HealthuU.BLL.Model;

//namespace HealthyU.Tests;


//public class RecipeImportService_BadTests
//{
//    [Fact]
//    public async Task ImportRecipesAsync_OverMocked_Simple()
//    {
//        var mockService = new Mock<IRecipeService>();
//        var mockFactory = new Mock<IHttpClientFactory>();
//        var mockRepo = new Mock<IRecipeRepository>();
//        var mockLogger = new Mock<ILoggerService<FileResourceHolder>>();

//        const string json = @"{""results"":[{""name"":""A""},{""name"":""B""}]}";
//        var client = new HttpClient(new FakeHttpHandler(json));

//        mockFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
//                   .Returns(client);

//        mockService.Setup(s => s.GetAllBaseRecipeData())
//                   .ReturnsAsync(Result.Success(new List<RecipeDTO>()));

//        var svc = new RecipeImportService(
//            mockService.Object,
//            mockFactory.Object,
//            mockRepo.Object,
//            mockLogger.Object);

//        // ACT
//        await svc.ImportRecipesAsync();

//        // ASSERT

//        //mockFactory.Verify(f => f.CreateClient(It.IsAny<string>()), Times.Once);

//        //mockService.Verify(s => s.GetAllBaseRecipeData(), Times.Once);

//        //mockService.Verify(s => s.CreateRecipeAsync(It.IsAny<RecipeDTO>()), Times.Exactly(2));

//        mockRepo.VerifyNoOtherCalls();
//        mockLogger.VerifyNoOtherCalls();
//    }
//}
