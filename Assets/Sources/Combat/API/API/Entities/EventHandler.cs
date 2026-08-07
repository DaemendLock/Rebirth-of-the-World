using Combat.API.Contexts;
using Combat.Common.ValueObjects;

using System;

namespace Combat.API
{
    public sealed class EventHandler : IDisposable
    {
        public EventHandlerId _id;
        public IEventContext _context;

        public EventHandler(EventHandlerId id, IEventContext context)
        {
            _id = id;
            _context = context;
        }

        public EventHandlerId Id => _id;

        public void Dispose()
        {
            _context.Unsubscribe(_id);
        }
    }
}
