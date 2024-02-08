using System;
using System.Collections.Generic;

namespace Core.Combat.Abilities.ActionRecords
{

    public interface IActionRecordContainer
    {
        void AddAction(ActionRecord action);
    }

    public class ActionRecordContainer : IActionRecordContainer
    {
        private List<ActionRecord> _actions;

        public void AddAction(ActionRecord action)
        {
            if (_actions == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _actions.Add(action);
        }
    }
}