namespace PowerDiary.ChatRoom.Core.DTOs
{
    public class ChatEventGroupDto
    {
        public string Time { get; set; }
        public IEnumerable<ChatEventGroupItemDto> ChatEvents { get; set; }
        public List<string> ChatEventDescriptions { get; set; }

    }
}
