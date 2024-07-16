using System.ClientModel;
using OpenAI.Chat;
using TalkToMe.Core.OpenAI.Enums;
using TalkToMe.Core.OpenAI.Models;

namespace TalkToMe.Core.OpenAI.Services;

public class ChatService
{
    private readonly ChatClient _chatClient;

    public ChatService(string apiKey, string model)
    {
        _chatClient = new ChatClient(model: model, credential: new ApiKeyCredential(apiKey));
    }
    
    public async Task<(ClientResult<ChatCompletion> Result, List<ChatMessage> Messages)> PerformChatAsync(List<ChatMessageContainer> conversationHistory)
    {
        var messages = PrepareMessagesForChat(conversationHistory); // Assume this method converts containers to messages.
        var response = await _chatClient.CompleteChatAsync(messages); // This sends the chat messages and waits for a response.
        return (response, messages); // Return the response along with the messages sent.
    }
    
    public List<ChatMessage> PrepareMessagesForChat(List<ChatMessageContainer> conversationHistory)
    {
        List<ChatMessage> chatMessages = new List<ChatMessage>();

        foreach (var message in conversationHistory)
        {
            foreach (var messageContent in message.MessageContent)
            {
                switch (message.MessageType)
                {
                    case ChatMessageType.System:
                        chatMessages.Add(new SystemChatMessage(messageContent.Message));
                        break;
                    case ChatMessageType.User:
                        chatMessages.Add(new UserChatMessage(messageContent.Message));
                        break;
                    case ChatMessageType.Assistant:
                        chatMessages.Add(new AssistantChatMessage(messageContent.Message));
                        break;
                }
            }
        }

        return chatMessages;
    }
    
    public void ProcessChatResponses(ClientResult<ChatCompletion>? response, List<ChatMessageContainer> conversationHistory)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        if (response != null && response.Value.Content.Count > 0)
        {
            foreach (var message in response.Value.Content)
            {
                Console.WriteLine($"Bot: {message.Text}");
                var botMessage = new ChatMessageContainer(new SystemChatMessage(message.Text));
                conversationHistory.Add(botMessage);
            }
        }
        else
        {
            Console.WriteLine("No response from chatbot.");
        }
        
        Console.ResetColor();
        
    }
}
