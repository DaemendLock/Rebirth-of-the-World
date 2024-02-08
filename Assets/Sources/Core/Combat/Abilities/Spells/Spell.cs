using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Combat.Utils.Serialization;
using Data.Spells;
using System.Collections.Generic;
using Utils.DataTypes;

namespace Core.Combat.Abilities
{
    public abstract class Spell : Castable
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

        public float CastTime => _data.CastTime;

        public float GCD => _data.GCD;

        public GcdCategory GcdCategory => _data.GcdCategory;

        public SchoolType School => _data.School;

        public SpellFlags Flags => _data.Flags;

        public int EffectsCount => _data.Effects.Length;

        //Server cast(data) -> Send(data) -> Client cast(data from server)

        public abstract CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values);

        public abstract CommandResult CanCast(Unit data, SpellValueProvider values);

        public bool IsDriving(Spell spell)
        {
            if (spell.Id == Id)
            {
                return true;
            }

            return false;
        }

        public float GetEffectValue(int index, float modifyValue) => _data.Effects[index].GetValue(modifyValue);

        public ActionRecord ApplyEffect(int index, float modification, Unit caster, Unit target) => _data.Effects[index].ApplyEffect(caster, target, modification);

        public float GetModifiedCastTime(PercentModifiedValue modification) => (new PercentModifiedValue(CastTime, 100) + modification).CalculatedValue;

        public sealed override int GetHashCode()
        {
            return Id;
        }

        public sealed override bool Equals(object obj)
        {
            return obj is Spell spell && spell.Id == Id;
        }

        public static Spell Get(SpellId id)
        {
            if (_loadedSpells.ContainsKey(id))
            {
                return _loadedSpells[id];
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
        protected static Team.Team GetSearchTeam(Unit caster, TargetTeam targetTeam) => targetTeam switch
        {
            TargetTeam.Both => Team.Team.TEAM_1 | Team.Team.TEAM_2,
            TargetTeam.Enemy => (Team.Team.TEAM_1 | Team.Team.TEAM_2) ^ caster.Team,
            TargetTeam.Ally => caster.Team,
            _ => Team.Team.NONE,
        };

        protected static float GetEffectiveRange(float defaultRange, SpellModification modification)
            => defaultRange * (100 + modification.BonusRange.Percent) / 100 + modification.BonusRange.BaseValue;
    }
}
