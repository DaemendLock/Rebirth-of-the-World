using Combat.Local.Domain.API;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.ValueObjects;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("Paladin1Aura")]
    public class Paladin1Aura : StatusApi, IOutgoingHealDamageHandler
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
            if (@event.Source.IsWeaponAttack == false)
            {
                return;
            }

            Unit caster = @event.Attacker;
            Unit target = @event.Target;

            _applyDamageOptions.Target = target;
            _applyDamageOptions.OriginalDamage = caster.GetAttributeValue(Attribute.Spellpower) * 0.1f;

            Parent.GiveResource(Source, new(2), 1);
        }
    }
}
