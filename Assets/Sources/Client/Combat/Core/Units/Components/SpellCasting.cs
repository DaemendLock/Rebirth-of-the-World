using Client.Combat.Core.Abilities;
using Client.Combat.Core.Utils;
using System;

namespace Client.Combat.Core.Units.Components
{
    public class SpellCast
    {
        public Duration Duration;
    }

    public class SpellCasting
    {
        public SpellCast ActiveCast { get; set; }

        public Ability GetAbility(int i)
        {
            throw new NotImplementedException();
        }
    }
}
