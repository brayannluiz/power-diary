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

        public ChatEventAppService(ChatEventRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<string> GetChatEventsMinuteByMinute()
        {
            var chatEvents = _repository.GetAll().OrderBy(c => c.Time).Select(c => c.ToString());

            return chatEvents;
        }

        public IEnumerable<ChatEventGroupDescriptionByTimeDto> GetChatEventsHourly()
        {
            var chatEventsGroupedByHour =
                from chatEvents in _repository.GetAll()
                orderby chatEvents.Time
                group chatEvents by chatEvents.Hour into groupedChatEventsByHour
                select new ChatEventGroupByTimeDto
                {
                    Time = groupedChatEventsByHour.Key,
                    ChatEvents = groupedChatEventsByHour.Select(chatEvent => new ChatEventGroupItemDto
                    {
                        ChatEventType = chatEvent.Type,
                        Count = groupedChatEventsByHour.Count(g => g.Type == chatEvent.Type),
                        UserCount = 
                            chatEvent.Type == ChatEventType.HighFiveAnotherUser ? groupedChatEventsByHour.Where(g => g.Type == chatEvent.Type).Select(g => g.User.Id).Distinct().Count() : 0,
                        UserInteractedWithCount = 
                            chatEvent.Type == ChatEventType.HighFiveAnotherUser ? groupedChatEventsByHour.Where(g => g.Type == chatEvent.Type).Select(g => g.UserInteractedWith.Id).Distinct().Count() : 0,
                    }),                    
                };

            var dto = chatEventsGroupedByHour.Select(e => new ChatEventGroupDescriptionByTimeDto
            {
                Time = e.Time,
                ChatEventDescriptions = ExtractChatEventDescriptions(e.ChatEvents)
            });
            
            return dto;
        }

        private List<string> ExtractChatEventDescriptions(IEnumerable<ChatEventGroupItemDto> chatEvents)
        {
            var messages = new List<string>();

            var messageForThoseWhoEnteredTheRoom = ExtractMessageForThoseWhoEnteredTheRoom(chatEvents);
            var messageForThoseWhoLeftTheRoom = ExtractMessageForThoseWhoLeftTheRoom(chatEvents);
            var messageForThoseWhoLeftComments = ExtractMessageForThoseWhoLeftComments(chatEvents);
            var messageForThoseWhoHighFivedOtherPeople = ExtractMessageForThoseWhoHighFivedOtherPeople(chatEvents);

            if (messageForThoseWhoEnteredTheRoom != null)
                messages.Add(messageForThoseWhoEnteredTheRoom);

            if (messageForThoseWhoLeftTheRoom != null)
                messages.Add(messageForThoseWhoLeftTheRoom);

            if (messageForThoseWhoLeftComments != null)
                messages.Add(messageForThoseWhoLeftComments);

            if (messageForThoseWhoHighFivedOtherPeople != null)
                messages.Add(messageForThoseWhoHighFivedOtherPeople);

            return messages;
        }

        private string ExtractMessageForThoseWhoEnteredTheRoom(IEnumerable<ChatEventGroupItemDto> chatEvents)
        {
            var dto = chatEvents.FirstOrDefault(chatEvent => chatEvent.ChatEventType == ChatEventType.EnterTheRoom);

            if (dto == null)
                return null;

            var treatmentForUser = dto.UserCount == 1 ? "person" : "people";

            return $"{dto.Count} {treatmentForUser} entered the room";
        }

        private string ExtractMessageForThoseWhoLeftTheRoom(IEnumerable<ChatEventGroupItemDto> chatEvents)
        {
            var dto = chatEvents.FirstOrDefault(chatEvent => chatEvent.ChatEventType == ChatEventType.LeaveTheRoom);

            if (dto == null)
                Enumerable.Empty<string>();

            return $"{dto.Count} left";
        }

        private string ExtractMessageForThoseWhoLeftComments(IEnumerable<ChatEventGroupItemDto> chatEvents)
        {
            var dto = chatEvents.FirstOrDefault(chatEvent => chatEvent.ChatEventType == ChatEventType.Comment);

            if (dto == null)
                return null;

            return $"{dto.Count} comments";
        }

        private string ExtractMessageForThoseWhoHighFivedOtherPeople(IEnumerable<ChatEventGroupItemDto> chatEvents)
        {
            var dto = chatEvents.FirstOrDefault(chatEvent => chatEvent.ChatEventType == ChatEventType.HighFiveAnotherUser);

            if (dto == null)
                return null;

            var treatmentForUser = dto.UserCount == 1 ? "person" : "people";
            var treatmentUserInteractedWith = dto.UserInteractedWithCount == 1 ? "person" : "people";


            return $"{dto.UserCount} {treatmentForUser} high-fived {dto.UserInteractedWithCount} {treatmentUserInteractedWith}";
        }
    }
}
