using System.Collections.Generic;

using Server.Combat.Domain.Actions;
using Server.Combat.Domain.Entities;
using Server.Combat.Infrastructure.Repositories;

namespace Server.Combat.Infrastructure.Implementations.Repositories
{
    public class ActionHandlerRepository : IActionHandlerRepository
    {
        private readonly Dictionary<int, IActionHandler> _actions;
        private readonly List<int> _removeBuffer;

        public ActionHandlerRepository()
        {
            _actions = new();
            _removeBuffer = new();
        }

        public void Add(IActionHandler actionHandler) => _actions.Add(actionHandler.Actor.Id.Value, actionHandler);

        public IActionHandler Get(Unit actor) => _actions.GetValueOrDefault(actor.Id.Value, null);

        public void Update(float deltaTime)
        {
            lock (_actions)
            {
                foreach (IActionHandler actionHandler in _actions.Values)
                {
                    actionHandler.ActiveTime += deltaTime;

                    if (actionHandler.IsActive == false)
                    {
                        _removeBuffer.Add(actionHandler.Actor.Id.Value);
                    }
                }

                foreach (int id in _removeBuffer)
                {
                    _actions.Remove(id);
                }

                _removeBuffer.Clear();
            }
        }
    }
}
