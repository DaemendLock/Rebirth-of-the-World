using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Team;
using Core.Combat.Units;
using Core.Combat.Utils;
using Data.Spells;
using System.Collections.Generic;
using Utils.DataTypes;
using Utils.Serializer;

namespace Core.Combat.Abilities
{
    public class Spell : Castable
    {
        private static Dictionary<SpellId, Spell> _loadedSpells = new Dictionary<SpellId, Spell>();

        private readonly SpellData _data;

        public Spell(SpellData data)
        {
            _data = data;
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

        public virtual void Cast(CastEventData data, SpellModification modification)
        {
            for (int i = 0; i < EffectsCount; i++)
            {
                ApplyEffect(i, modification.EffectsModifications[i], data);
            }
        }

        public virtual CommandResult CanCast(CastEventData data, SpellModification modification)
        {
            Unit caster = data.Caster;
            Unit target = data.Target;

            if (target == null)
            {
                return CommandResult.INVALID_TARGET;
            }

            float effectiveCastRange = Range * (1 + modification.BonusRange.Percent / 100) + modification.BonusRange.BaseValue;

            if ((caster.Position - target.Position).sqrMagnitude > effectiveCastRange * effectiveCastRange)
            {
                return CommandResult.OUT_OF_RANGE;
            }

            if (TargetTeam == TargetTeam.ALLY && (!caster.CanHelp(target)))
            {
                return CommandResult.INVALID_TARGET;
            }

            if (TargetTeam == TargetTeam.ENEMY && (!caster.CanHurt(target)))
            {
                return CommandResult.INVALID_TARGET;
            }

            return CommandResult.SUCCES;
        }

        public bool IsDriving(Spell spell)
        {
            if (spell.Id == Id)
            {
                return true;
            }

            return false;
        }

        public float GetEffectValue(int index, float modifyValue) => _effects[index].GetValue(modifyValue);

        public void ApplyEffect(int index, float modifyValue, CastEventData data) => _effects[index].ApplyEffect(data, modifyValue);

        public sealed override int GetHashCode()
        {
            return Id;
        }

        public sealed override bool Equals(object obj)
        {
            return obj is Spell spell && spell.Id == Id;
        }

        protected static Team.Team GetSearchTeam(Unit caster, TargetTeam targetTeam) => targetTeam switch
        {
            TargetTeam.BOTH => Team.Team.TEAM_1 | Team.Team.TEAM_2,
            TargetTeam.ENEMY => (Team.Team.TEAM_1 | Team.Team.TEAM_2) ^ caster.Team,
            TargetTeam.ALLY => caster.Team,
            _ => Team.Team.NONE,
        };

        protected static float GetEffectiveRange(float defaultRange, SpellModification modification)
            => defaultRange * (100 + modification.BonusRange.Percent) / 100 + modification.BonusRange.BaseValue;

        public static Spell Get(SpellId id)
        {
            if (_loadedSpells.ContainsKey(id))
            {
                return _loadedSpells[id];
            }

            if (SpellDataLoader.Loaded == false)
            {
                SpellDataLoader.Load();
            }

            Spell result = SpellSerializer.FromSpellData(SpellSerializer.Deserialize(SpellDataLoader.GetCombatSpell(id)));
            _loadedSpells[id] = result;
            return result;
        }

        public static void ReleaseLoadedSpells()
        {
            _loadedSpells.Clear();
        }
#if DEBUG
        public static void RegisterSpell(Spell spell)
        {
            _loadedSpells[spell.Id] = spell;
        }
#endif
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
