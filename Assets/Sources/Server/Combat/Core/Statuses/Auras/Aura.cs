using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Statuses.Auras.AuraEffects;
using Core.Combat.Units;
using System;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Combat.Statuses.Auras
{
    public class Aura
    {
        private static readonly Dictionary<SpellId, Aura> _auras = new Dictionary<SpellId, Aura>();

        public SpellId Id;
        public DispellType Type;
        public AuraFlags Flags;
        public int MaxStackCount;
        //public var Script;

        private readonly AuraEffect[] _effects;

        public void Apply(Unit target)
        {
            foreach (AuraEffect effect in _effects)
            {
                effect.ApplyEffect(target);
            }
        }

        public static Aura Get(SpellId id)
        {
            return _auras.GetValueOrDefault(id, null);
        }

        public void Update(IActionRecordContainer container)
        {
            throw new NotImplementedException();
            //return new AuraUpdateAction("AAAA");
        }
    }

    public class AuraApplicationData
    {
        public Unit Caster;
        public Unit Target;
    }
}