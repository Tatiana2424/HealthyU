using Azure;

using CSharpFunctionalExtensions;

using HealthuU.BLL.Services.Interfaces;

using HealthyU.DAL.Extensions;
using HealthyU.WebApi.Configurations;

using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using OpenAI_API.Chat;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Realizations
{
    public class OpenAIService : IOpenAIService
    {
        private readonly OpenAI _openAI;
        public OpenAIService(IOptionsMonitor<OpenAI> optionsMonitor) 
        {
            _openAI = optionsMonitor.CurrentValue;
        }

        public async Task<string> GetAnswer(string text)
        {
            //api instance
            var api = new OpenAI_API.OpenAIAPI(_openAI.Key);

            string promptText = OpenAIPrompt.HealthyOpenAIPrompt;


            ChatRequest chatRequest = new ChatRequest()
            {
                Model = "gpt-3.5-turbo",
                Temperature = 0.0,
                MaxTokens = 500,
                ResponseFormat = ChatRequest.ResponseFormats.Text,
                Messages = new ChatMessage[] {
                    new ChatMessage(ChatMessageRole.System, promptText),
                    new ChatMessage(ChatMessageRole.User, text)
                }
            };

            var results = await api.Chat.CreateChatCompletionAsync(chatRequest);

            return results.Choices.FirstOrDefault().Message.TextContent;

            //Thread.Sleep(8000);
            //var result = "Certainly! Here are some tips for healthy eating:\n\n1. Eat a **Variety of Foods**: Include a wide range of fruits, vegetables, whole grains, lean proteins, and healthy fats in your diet to ensure you get a variety of nutrients.\n\n2. **Watch Portion Sizes**: Be mindful of portion sizes to avoid overeating. Use smaller plates, bowls, and cups to help control portions.\n\n3. Limit Processed Foods: Try to minimize your intake of processed foods high in added sugars, salt, and unhealthy fats. Opt for whole, unprocessed foods whenever possible.\n\n4. Stay Hydrated: Drink plenty of water throughout the day. Limit sugary drinks and opt for water, herbal teas, or infused water instead.\n\n5. Balance Your Plate: Aim to fill half your plate with fruits and vegetables, one-quarter with lean protein, and one-quarter with whole grains.\n\n6. Snack Smart: Choose healthy snacks like fruits, vegetables, nuts, or yogurt instead of processed snacks high in sugar and unhealthy fats.\n\n7. Cook at Home: Cooking at home allows you to control the ingredients and cooking methods, making it easier to eat healthier.\n\n8. Read Labels: Pay attention to food labels and ingredients lists to make informed choices about the foods you consume.\n\n9. Practice Mindful Eating: Eat slowly, savor your food, and pay attention to hunger and fullness cues to prevent overeating.\n\n10. Plan Ahead: Plan your meals and snacks in advance to avoid impulsive, unhealthy choices when you're hungry.\n\nRemember, healthy eating is about balance and moderation. It's important to enjoy your food while nourishing your body with the nutrients it needs.";

           // return result;
        }
    }
}
