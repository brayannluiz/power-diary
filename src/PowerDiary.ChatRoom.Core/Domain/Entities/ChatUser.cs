using PowerDiary.ChatRoom.Core.Domain.ValueObjects;

namespace PowerDiary.ChatRoom.Core.Domain.Entities
{
    public class ChatUser
    {
        private ChatUser(ChatUserId id, string name)
        {
            Id = id;
            Name = name;
        }

        public ChatUserId Id { get; }
        public string Name { get; }

        public static ChatUser Create(string name) => new(ChatUserId.New(), name);
    }
}
