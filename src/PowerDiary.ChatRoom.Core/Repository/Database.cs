using PowerDiary.ChatRoom.Core.Domain.Entities;

namespace PowerDiary.ChatRoom.Core.Repository
{
    internal class Database
    {
        internal static IQueryable<ChatEvent> ChatEvents
        {
            get
            {
                var bob = ChatUser.Create("Bob");
                var kate = ChatUser.Create("Kate");

                // chatUsers to validate the high five logic
                //var jenny = ChatUser.Create("Jenny");
                //var joe = ChatUser.Create("Joe");

                var chatEvents = new List<ChatEvent>
                {
                    ChatEvent.EnterTheRoomEvent(bob, new TimeOnly(17, 0)),
                    ChatEvent.EnterTheRoomEvent(kate, new TimeOnly(17, 5)),
                    ChatEvent.CommentEvent(bob, new TimeOnly(17, 15), "Hey, Kate - high five?"),
                    ChatEvent.HighFiveAnotherUserEvent(kate, bob, new TimeOnly(17, 17)),

                    // events to validate the high five logic
                    //ChatEvent.HighFiveAnotherUserEvent(kate, jenny, new TimeOnly(17, 17)),
                    //ChatEvent.HighFiveAnotherUserEvent(kate, joe, new TimeOnly(17, 17)),

                    ChatEvent.LeaveTheRoomEvent(bob, new TimeOnly(17, 18)),
                    ChatEvent.CommentEvent(kate, new TimeOnly(17, 20), "Oh, typical"),
                    ChatEvent.LeaveTheRoomEvent(kate, new TimeOnly(17, 21))
                };

                return chatEvents.AsQueryable();
            }
        }
    }

}
