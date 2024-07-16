using OpenAI.Chat;
using TalkToMe.Core.OpenAI.Enums;

namespace TalkToMe.Core.OpenAI.Models;

[Serializable]
public class ChatMessageContainer
{
    public ChatMessageType MessageType { get; set; }
    public IList<ChatMessageContent> MessageContent { get; set; }
    public string? Name { get; set; }

    public ChatMessageContainer()
    {
        MessageContent = new List<ChatMessageContent>();
    }

    public ChatMessageContainer(ChatMessageType messageType, IList<ChatMessageContent> messageContent, string name)
    {
        MessageType = messageType;
        MessageContent = messageContent;
        Name = name;
    }

    public ChatMessageContainer(ChatMessage chatMessage, string? name = null)
    {
        MessageContent = new List<ChatMessageContent>();
        foreach (var content in chatMessage.Content)
        {
            MessageContent.Add(new ChatMessageContent(content.Text));
        }
        
        switch (chatMessage)
        {
            case SystemChatMessage systemChatMessage:
                MessageType = ChatMessageType.System;
                Name = systemChatMessage.ParticipantName;
                break;
            case UserChatMessage userChatMessage:
                MessageType = ChatMessageType.User;
                Name = userChatMessage.ParticipantName;
                break;
            case AssistantChatMessage assistantChatMessage:
                MessageType = ChatMessageType.Assistant;
                Name = assistantChatMessage.ParticipantName;
                break;
        }
        
        if(name != null)
        {
            Name = name;
        }
    }
    
    public override string ToString()
    {
        return $"{MessageType}: {MessageContent}";
    }
}