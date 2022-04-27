using PowerDiary.ChatRoom.Core.Domain.ValueObjects;

namespace PowerDiary.ChatRoom.Core.DTOs
{
    public class ChatEventGroupItemDto
    {
        public ChatEventType ChatEventType { get; set; }
        public int Count { get; set; }
        public int? UserCount { get; set; }
        public int? UserInteractedWithCount { get; set; }
    }
}
