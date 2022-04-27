namespace PowerDiary.ChatRoom.Core.DTOs
{
    public class ChatEventGroupByTimeDto
    {
        public string Time { get; set; }
        public IEnumerable<ChatEventGroupItemDto> ChatEvents { get; set; }
    }
}
