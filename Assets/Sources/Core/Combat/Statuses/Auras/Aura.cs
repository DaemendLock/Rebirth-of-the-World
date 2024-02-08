using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Units;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Combat.Statuses.Auras
{
    public class Aura
    {
        private static readonly Dictionary<SpellId, Aura> _auras = new Dictionary<SpellId, Aura>();

        public SpellId Id;
        public float Duration;
        public DispellType Type;
        public AuraFlags Flags;
        public int MaxStackCount;
        //public var Script;

        private readonly AuraEffect[] _effects;

        public static Aura Get(SpellId id)
        {
            return _auras.GetValueOrDefault(id, null);
        }
    }

    public class AuraApplicationData
    {
        public Unit Caster;
        public Unit Target;
    }
}