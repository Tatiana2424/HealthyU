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
        }
    }
}
