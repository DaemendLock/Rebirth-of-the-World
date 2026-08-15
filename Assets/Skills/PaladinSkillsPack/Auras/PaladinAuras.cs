using Combat.API;
using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Statuses;
using Combat.API.Utils;
using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("Paladin1Aura")]
    public class Paladin1Aura : StatusScript, IOutgoingHealDamageHandler
    {
        private ApplyDamageOptions _applyDamageOptions;

        public override void OnCreate()
        {
            _applyDamageOptions = new()
            {
                Attacker = Parent,
                Source = Source,
                Flags = DamageFlags.None,
            };
        }

        public void OnDealDamage(DamageRecord @event)
        {
            if (@event.Flags.HasFlag(DamageFlags.WeaponAttack) == false)
            {
                return;
            }

            Unit caster = @event.Attacker;
            Unit target = @event.Target;

            _applyDamageOptions.Target = target;
            _applyDamageOptions.OriginalDamage = caster.GetAttributeValue(UnitAttribute.Spellpower) * 0.1f;
            UnityEngine.Debug.Log("+Smite Hit");
            Parent.GiveResource(new(ResourceId.Custom, 10, Source));
        }
    }
}
