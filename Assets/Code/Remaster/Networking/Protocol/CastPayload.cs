using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaster.Networking.Protocol
{
    public class CastPayload
    {
        public readonly int Caster;
        public readonly int Target;
        public readonly byte SpellSlot;
        public readonly long Time;

        public CastPayload(int caster, int target, byte spellSlot, long time)
        {
            Caster = caster;
            Target = target;
            SpellSlot = spellSlot;
            Time = time;
        }
    }
}
