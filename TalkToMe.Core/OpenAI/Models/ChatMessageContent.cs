namespace TalkToMe.Core.OpenAI.Models;

[Serializable]
public struct ChatMessageContent
{
    public string Message { get; set; }

    public ChatMessageContent(string message)
    {
        Message = message;
    }
}