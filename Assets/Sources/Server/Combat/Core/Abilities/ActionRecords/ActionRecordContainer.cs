using System;
using System.Collections.Generic;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Abilities.ActionRecords
{
    public interface IActionRecordContainer
    {
        void AddAction(ActionRecord action);

        void Write(BinaryWriter target);
    }

    public class ActionRecordContainer : IActionRecordContainer
    {
        private List<ActionRecord> _actions = new();

        public void AddAction(ActionRecord action)
        {
            if (_actions == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _actions.Add(action);
        }

        public void Write(BinaryWriter target)
        {
            foreach (ActionRecord action in _actions)
            {
                if (action.Type == ActionType.Dummy)
                {
                    continue;
                }

                target.Write((byte) action.Type);

                target.Write(action.Target);
                target.Write(action.Source);
                target.Write(action.GetBytes());
            }
        }
    }
}