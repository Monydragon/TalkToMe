using Newtonsoft.Json;
using TalkToMe.Core.OpenAI.Models;

public class ConversationManager
{
    private readonly string _baseFolder;

    public ConversationManager(string baseFolder = "Conversations")
    {
        _baseFolder = Path.Combine(Directory.GetCurrentDirectory(), baseFolder);
        Directory.CreateDirectory(_baseFolder); // Ensure the directory exists
    }

    private JsonSerializerSettings JsonSerializerSettings => new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented,
    };

    public void SaveConversation(List<ChatMessageContainer> conversationHistory, string conversationName)
    {
        string fileName = $"{conversationName}.json";
        string filePath = Path.Combine(_baseFolder, fileName);
        string json = JsonConvert.SerializeObject(conversationHistory, JsonSerializerSettings);
        File.WriteAllText(filePath, json);
    }

    public List<ChatMessageContainer> LoadConversation(string fileName)
    {
        string filePath = Path.Combine(_baseFolder, fileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<ChatMessageContainer>>(json, JsonSerializerSettings) ?? new List<ChatMessageContainer>();
        }
        return new List<ChatMessageContainer>();
    }

    public bool DeleteConversation(string fileName)
    {
        string filePath = Path.Combine(_baseFolder, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }
        return false;
    }

    public Dictionary<int, string> ListConversations()
    {
        var files = Directory.GetFiles(_baseFolder, "*.json");
        Dictionary<int, string> conversations = new Dictionary<int, string>();
        int index = 1;
        foreach (var file in files)
        {
            conversations[index++] = Path.GetFileName(file);
        }
        return conversations;
    }
}
