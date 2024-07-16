using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using TalkToMe.Core.Configuration.Services;
using TalkToMe.Core.OpenAI.Models;
using TalkToMe.Core.OpenAI.Services;

namespace TalkToMe.ConsoleUI;

class Program
{
    static async Task Main(string[] args)
    {
        // Initialize services
        ConfigService configService = new ConfigService();
        IConfiguration config = configService.LoadConfiguration();
        
        string apiKey = config["OpenAI:ApiKey"];
        string model = config["OpenAI:Model"];

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(model))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("API key or model is missing in appsettings.json");
            Console.ResetColor();
            return;
        }
        
        ChatService chatService = new ChatService(apiKey, model);
        ConversationManager conversationManager = new ConversationManager();
        
        // Retrieve or start a new conversation
        List<ChatMessageContainer> conversationHistory = new List<ChatMessageContainer>();
        string conversationName = "";
        bool shouldExit = false;

        while (!shouldExit)
        {
            var conversations = conversationManager.ListConversations();
            if (conversations.Any())
            {
                Console.WriteLine("Available conversations:");
                foreach (var entry in conversations)
                {
                    Console.WriteLine($"{entry.Key}: {entry.Value}");
                }

                Console.WriteLine("Choose an option: [new/load/delete/exit]");
                string choice = Console.ReadLine().Trim().ToLower();

                switch (choice)
                {
                    case "new":
                        Console.WriteLine("Enter a name for the new conversation:");
                        conversationName = Console.ReadLine().Trim();
                        conversationHistory = new List<ChatMessageContainer>();
                        shouldExit = true;
                        break;
                    case "load":
                        Console.WriteLine("Enter the number of the conversation to load:");
                        if (int.TryParse(Console.ReadLine(), out int loadChoice) && conversations.ContainsKey(loadChoice))
                        {
                            conversationHistory = conversationManager.LoadConversation(conversations[loadChoice]);
                            conversationName = conversations[loadChoice];
                            shouldExit = true;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid conversation number. Please try again.");
                            Console.ResetColor();
                        }
                        break;
                    case "delete":
                        Console.WriteLine("Enter the number of the conversation to delete:");
                        if (int.TryParse(Console.ReadLine(), out int deleteChoice) && conversations.ContainsKey(deleteChoice))
                        {
                            if (conversationManager.DeleteConversation(conversations[deleteChoice]))
                                Console.WriteLine("Conversation deleted successfully.");
                            else
                                Console.WriteLine("Failed to delete conversation.");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid conversation number. Please try again.");
                            Console.ResetColor();
                        }
                        break;
                    case "exit":
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Invalid option. Please enter one of the following: new, load, delete, exit.");
                        Console.ResetColor();
                        break;
                }
            }
            else
            {
                Console.WriteLine("No conversations available. Starting a new conversation.");
                Console.WriteLine("Enter a name for the new conversation:");
                conversationName = Console.ReadLine().Trim();
                conversationHistory = new List<ChatMessageContainer>();
                shouldExit = true;
            }
        }
        
        Console.WriteLine("Chatbot is ready. Type 'exit' to quit.");
        Console.ForegroundColor = ConsoleColor.Cyan;  // Set user message color
        while (true)
        {
            Console.Write("You: ");
            string input = Console.ReadLine();
            if (input.ToLower() == "exit")
            {
                conversationManager.SaveConversation(conversationHistory, conversationName);
                break;
            }
            
            try
            {
                var userMessage = new ChatMessageContainer(new UserChatMessage(input));
                conversationHistory.Add(userMessage);
                
                var response = await chatService.PerformChatAsync(conversationHistory);
                chatService.ProcessChatResponses(response.Result, conversationHistory);
                
                conversationManager.SaveConversation(conversationHistory, conversationName);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
        }
        Console.ForegroundColor = ConsoleColor.Cyan;  // Reset default color
    }
}
