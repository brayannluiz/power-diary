using PowerDiary.ChatRoom.Core.Domain.ValueObjects;
using PowerDiary.ChatRoom.Core.DTOs;
using PowerDiary.ChatRoom.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDiary.ChatRoom.Core.Application
{
    public class ChatEventAppService
    {
        private readonly ChatEventRepository _repository;

        public ChatEventAppService()
        {
            _repository = new ChatEventRepository();
        }

        public IEnumerable<string> GetChatEventsMinuteByMinute()
        {
            var chatEvents = _repository.GetAll().OrderBy(c => c.Time).Select(c => c.ToString());

            return chatEvents;
        }

        public IEnumerable<ChatEventGroupDto> GetChatEventsHourly()
        {
            var chatEventsGroupedByHour =
                from chatEvents in _repository.GetAll()
                orderby chatEvents.Time
                group chatEvents by chatEvents.Hour into groupedChatEventsByHour
                select new ChatEventGroupDto
                {
                    Time = groupedChatEventsByHour.Key,
                    ChatEvents = groupedChatEventsByHour.Select(chatEvent => new ChatEventGroupItemDto
                    {
                        ChatEventType = chatEvent.Type,
                        Count = groupedChatEventsByHour.Count(g => g.Type == chatEvent.Type)
                    }),                    
                };

            foreach (var chatEventGroupedByHour in chatEventsGroupedByHour.ToList())
            {
                chatEventGroupedByHour.ChatEventDescriptions = new List<string>();
                chatEventGroupedByHour.ChatEventDescriptions.AddRange(ExtractChatEventDescriptions(chatEventGroupedByHour.ChatEvents));
            }

            return chatEventsGroupedByHour;
        }

        private List<string> ExtractChatEventDescriptions(IEnumerable<ChatEventGroupItemDto> chatEvents)
        {
            var messages = new List<string>();

            var messageForThoseWhoEnteredTheRoom = ExtractMessageForThoseWhoEnteredTheRoom(chatEvents);
            var messageForThoseWhoLeftTheRoom = ExtractMessageForThoseWhoLeftTheRoom(chatEvents);

            if (messageForThoseWhoEnteredTheRoom != null)
                messages.Add(messageForThoseWhoEnteredTheRoom);

            if (messageForThoseWhoLeftTheRoom != null)
                messages.Add(messageForThoseWhoLeftTheRoom);

            return messages;
        }

        private string ExtractMessageForThoseWhoEnteredTheRoom(IEnumerable<ChatEventGroupItemDto> chatEvents)
        {
            var dto = chatEvents.FirstOrDefault(chatEvent => chatEvent.ChatEventType == ChatEventType.EnterTheRoom);

            if (dto == null)
                return null;

            return $"{dto.Count} entered the room";
        }

        private string ExtractMessageForThoseWhoLeftTheRoom(IEnumerable<ChatEventGroupItemDto> chatEvents)
        {
            var dto = chatEvents.FirstOrDefault(chatEvent => chatEvent.ChatEventType == ChatEventType.LeaveTheRoom);

            if (dto == null)
                Enumerable.Empty<string>();

            return $"{dto.Count} left";
        }

    }
}
