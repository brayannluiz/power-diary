using PowerDiary.ChatRoom.Core.Domain.Entities;
using System.Linq.Expressions;

namespace PowerDiary.ChatRoom.Core.Repository
{
    public class ChatEventRepository
    {
        public IEnumerable<ChatEvent> GetAll(Expression<Func<ChatEvent, bool>> where = null)
        {
            if (where is null)
                return Database.ChatEvents.AsEnumerable();

            return Database.ChatEvents.Where(where).AsEnumerable();
        }
    }

}
