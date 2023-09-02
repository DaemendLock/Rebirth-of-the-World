using Core.Combat.Utils;
using System.Collections.Generic;

namespace Core.Sync
{
    public static class ModelSync
    {
        private static Queue<EventData> _castQueue;
        private static Queue<EventData> _receiveQueue;

        public static void Synchronize()
        {
            lock (_castQueue)
            {
                while (_castQueue.Count != 0)
                {
                    EventData input = _castQueue.Dequeue();
                    input.Caster.CastAbility(input);
                }
            }
        }
    }
}
