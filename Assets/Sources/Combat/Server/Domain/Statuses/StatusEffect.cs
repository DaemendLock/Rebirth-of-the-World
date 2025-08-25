using Server.Combat.Domain.Attributes;
using Server.Combat.Domain.Events;
using Server.Combat.Domain.Units.ValueObjects;
using Server.Combat.Domain.ValueObjects.Statuses;

namespace Server.Combat.Domain.Entities
{

    public class StatusEffect
    {
        public StatusId Id { get; }
        public Unit Parent { get; }

        public int StackCount { get; set; }
        public float Duration { get; set; }

        public void ModifyDamageDealth(DamageEvent @event)
        {

        }

        public void ModifyDamageRecive(DamageEvent @event)
        {

        }

        public void OnTakeDamage(DamageInstance damage)
        {

        }

        public void OnDealDamage(DamageInstance damage)
        {

        }

        public void ModifyAttributes(IAttributeCollection<Attribute> values)
        {

        }
    }
}
