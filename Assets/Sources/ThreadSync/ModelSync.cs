using Core.Combat.Utils;
using System.Collections.Generic;

namespace Core.Sync
{
    public static class ModelSync
    {
        private static Queue<CastEventData> _castQueue;
        private static Queue<CastEventData> _receiveQueue;

        public static void Synchronize()
        {
            lock (_castQueue)
            {
                while (_castQueue.Count != 0)
                {
                    CastEventData input = _castQueue.Dequeue();
                    input.Caster.CastAbility(input);
                }
            }
        }
    }
}
