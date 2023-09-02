using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Utils;
using Utils.DataTypes;

namespace Core.Combat.Abilities
{
    public class Spell : Castable
    {
        private readonly SpellData _data;

        public Spell(SpellData data)
        {
            _data = data;
            SpellLibrary.SpellLib.RegisterSpell(this);
        }

#if UNITY_EDITOR
        public SpellData Data => _data;
#endif

        public SpellId Id => _data.Id;

        public AbilityCost Cost => _data.Cost;

        public TargetTeam TargetTeam => _data.TargetTeam;

        public float Range => _data.Range;

        public float Cooldown => _data.Cooldown;

        public float Duration => _data.Duration;

        public float CastTime => _data.CastTime;

        public float GCD => _data.GCD;

        public GcdCategory GcdCategory => _data.GcdCategory;

        public SchoolType School => _data.School;

        public SpellFlags Flags => _data.Flags;

        public int EffectsCount => _effects.Length;

        private SpellEffect[] _effects => _data.Effects;

        public virtual void Cast(EventData data, SpellModification modification)
        {
            for (int i = 0; i < EffectsCount; i++)
            {
                ApplyEffect(i, modification.EffectsModifications[i], data);
            }

            data.Caster?.InformCast(data, CommandResult.SUCCES);
        }

        public virtual CommandResult CanCast(EventData data, SpellModification modification) => CommandResult.SUCCES;

        public bool IsDriving(Spell spell)
        {
            if (spell.Id == Id)
            {
                return true;
            }

            return false;
        }

        public float GetEffectValue(int index, float modifyValue) => _effects[index].GetValue(modifyValue);

        public void ApplyEffect(int index, float modifyValue, EventData data) => _effects[index].ApplyEffect(data, modifyValue);

        public sealed override int GetHashCode()
        {
            return Id;
        }

        public sealed override bool Equals(object obj)
        {
            return obj is Spell spell && spell.Id == Id;
        }
    }

    public class SpellModification
    {
        public AbilityCost BonusCost;
        public PercentModifiedValue BonusRange;
        public PercentModifiedValue BonusCastTime;
        public PercentModifiedValue CooldownReduction;
        public PercentModifiedValue BonusDuration;
        public readonly float[] EffectsModifications;

        public SpellModification(Spell spell)
        {
            EffectsModifications = new float[spell.EffectsCount];
        }
    }
}
