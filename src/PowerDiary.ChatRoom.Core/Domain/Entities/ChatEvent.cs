using PowerDiary.ChatRoom.Core.Domain.ValueObjects;

namespace PowerDiary.ChatRoom.Core.Domain.Entities
{
    public class ChatEvent
    {
        protected ChatEvent() { }

        private ChatEvent(ChatEventId id, ChatEventType type, ChatUser user, ChatUser userInteractedWith, TimeOnly time, string content)
        {
            Id = id;
            Type = type;
            User = user;
            UserInteractedWith = userInteractedWith;
            Time = time;
            Content = content;
        }

        public ChatEventId Id { get; }
        public ChatEventType Type { get; }
        public virtual ChatUser User { get; }
        public ChatUser UserInteractedWith { get; set; }

        public virtual TimeOnly Time { get; }
        public string Content { get; set; }
        public string HourAndMinute => $"{Time:hh}:{Time:mm}{(Time.Hour >= 12 ? "pm" : "am")}";
        public string Hour => $"{Time:hh}{(Time.Hour >= 12 ? "pm" : "am")}";

        public override string ToString() => Type switch
        {
            ChatEventType.EnterTheRoom        => $"{HourAndMinute}: {User.Name} enters the room",
            ChatEventType.LeaveTheRoom        => $"{HourAndMinute}: {User.Name} leaves",
            ChatEventType.Comment             => $"{HourAndMinute}: {User.Name} comments: {Content}",
            ChatEventType.HighFiveAnotherUser => $"{HourAndMinute}: {User.Name} high-fives {UserInteractedWith.Name}",

            _ => throw new ArgumentException("Chat Event type not valid."),
        };

        public static ChatEvent EnterTheRoomEvent(ChatUser user, TimeOnly time)
        { 
            return new(ChatEventId.New(), ChatEventType.EnterTheRoom, user, null, time, null);
        }

        public static ChatEvent LeaveTheRoomEvent(ChatUser user, TimeOnly time)
        {
            return new(ChatEventId.New(), ChatEventType.LeaveTheRoom, user, null, time, null);
        }

        public static ChatEvent CommentEvent(ChatUser user, TimeOnly time, string content)
        {
            return new(ChatEventId.New(), ChatEventType.Comment, user, null, time, content);
        }

        public static ChatEvent HighFiveAnotherUserEvent(ChatUser user, ChatUser userInteractedWith, TimeOnly time)
        {
            return new(ChatEventId.New(), ChatEventType.HighFiveAnotherUser, user, userInteractedWith, time, null);
        }
    }
}
