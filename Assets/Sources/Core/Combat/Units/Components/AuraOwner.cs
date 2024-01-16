using Core.Combat.Abilities;
using Core.Combat.Statuses;
using Core.Combat.Interfaces;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using System;
using System.Collections.Generic;
using Utils.DataStructure;
using Utils.DataStructures;
using Utils.DataTypes;
using Core.Combat.Statuses.Auras;

namespace Core.Combat.Units.Components
{
    public class AuraOwner : DynamicStatOwner, Damageable
    {
        private List<Status> _statuses = new List<Status>();

        private Dictionary<Mechanic, int> _immunes = new Dictionary<Mechanic, int>();
        private ArrayQueue<int> _delete = new(16);

        public void ApplyAura(CastEventData data, Aura aura)
        {
            Status status = FindStatus(aura);

            if (aura.Flags.HasFlag(AuraFlags.SeparateStatuses) || status == null)
            {
                status = AddStatus(data.Target, aura);
            }

            status.TryApplyCaster(data.Caster);
            //status.AddEffect(effect);
        }

        public void RemoveStatus(Aura spell)
        {
            Status removableStatus = FindStatus(spell);

            if (removableStatus == null)
            {
                return;
            }

            removableStatus.ClearEffects();
            MarkUpDelete(spell.Id);
        }

        public void RemoveStatus(SpellId spell)
        {
            Status removableStatus = FindStatus(spell);

            if (removableStatus == null)
            {
                return;
            }

            removableStatus.Remove();
        }

        public bool HasStatus(Aura aura) => FindStatus(aura) != null;

        public bool HasStatus(SpellId spell) => FindStatus(spell) != null;

        public void Dispell(DispellType type)
        {
            foreach (Status statuses in _statuses)
            {
                throw new NotImplementedException();
            }
        }

        public void Purge()
        {
            throw new NotImplementedException();
        }

        public Status FindStatus(Aura aura)
        {
            foreach (Status status in _statuses)
            {
                throw new NotImplementedException();
                if (status.Spell == spell)
                {
                    return status;
                }
            }

            return null;
        }

        public Status FindStatus(SpellId spell)
        {
            foreach (Status status in _statuses)
            {
                if (status.Spell.Id == spell)
                {
                    return status;
                }
            }

            return null;
        }

        public PercentModifiedValue EvaluateStat(UnitStat stat)
        {
            PercentModifiedValue result = new PercentModifiedValue();

            foreach (Status status in _statuses)
            {
                result += status.EvaluateStat(stat);
            }

            return result;
        }

        public void TakeDamage(DamageEvent @event)
        {
            for (int i = 0; i < _statuses.Count; i++)
            {
                _statuses[i].TakeDamage(@event);
            }
        }

        public void AbsorpDamage(DamageEvent @event)
        {
            float eventDamage = @event.EvaluateDamage();

            foreach (Status status in _statuses)
            {
                eventDamage -= status.AbsorbDamage(@event, eventDamage);

                if (eventDamage == 0)
                {
                    return;
                }
            }
        }

        public void AbsorbDamage(CastEventData data, float absorption, SchoolType school)
        {
            throw new NotImplementedException();
            Status status = FindStatus(data.Spell) ?? AddStatus(data.Target, data.Spell);
            status.TryApplyCaster(data.Caster);
            status.GiveAbsorption(new(absorption, school));
        }

        public float GetAbsorption()
        {
            float result = 0;

            foreach (Status status in _statuses)
            {
                result += status.Absorption;
            }

            return result;
        }

        public void Update()
        {
            lock (this)
            {
                for (int i = 0; i < _statuses.Count; i++)
                {
                    if (_statuses[i].Expired)
                    {
                        _statuses[i].ClearEffects();
                        MarkUpDelete(i);
                    }
                    else
                    {
                        _statuses[i].Update();
                    }
                }

                Flush();
            }
        }

        public void InformAction(UnitAction action, CastEventData data)
        {
            foreach (Status status in _statuses)
            {
                status.CallAction(action, data);
            }
        }

        public int ImmunityValue(Mechanic mechanic)
        {
            if (_immunes.ContainsKey(mechanic) == false)
            {
                return 0;
            }

            return _immunes[mechanic];
        }

        private Status AddStatus(Unit target, Aura aura)
        {
            //Status result = new Status(target, spell);
            //_statuses.Add(result);
            throw new NotImplementedException();
            return result;
        }

        private void MarkUpDelete(int spellId)
        {
            _delete.Enqueue(spellId);
        }

        private void Flush()
        {
            while (_delete.Empty == false)
            {
                _statuses.RemoveAt(_delete.Dequeue());
            }
        }
    }
}