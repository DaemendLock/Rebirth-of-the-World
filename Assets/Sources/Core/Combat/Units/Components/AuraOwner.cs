using Core.Combat.Abilities;
using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine;
using Core.Combat.Statuses;
using Core.Combat.Statuses.Auras;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using System;
using System.Collections.Generic;
using Utils.DataStructure;
using Utils.DataStructures;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    internal class AuraOwner : Updatable
    {
        private List<Status> _statuses = new List<Status>();

        private Dictionary<Mechanic, int> _immunes = new Dictionary<Mechanic, int>();
        private ArrayQueue<int> _delete = new(16);

        internal void ApplyAura(CastInputData data, Aura aura)
        {
            Status status = FindStatus(aura);

            if (aura.Flags.HasFlag(AuraFlags.SeparateStatuses) || status == null)
            {
                status = AddStatus(data.Target, aura);
            }

            status.TryApplyCaster(data.Caster);
            //status.AddEffect(effect);
        }

        internal void RemoveStatus(Aura aura)
        {
            Status removableStatus = FindStatus(aura);

            if (removableStatus == null)
            {
                return;
            }

            removableStatus.ClearEffects();
            MarkUpDelete(aura.Id);
        }

        internal void RemoveStatus(SpellId spell)
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

        internal void Dispell(DispellType type)
        {
            foreach (Status statuses in _statuses)
            {
                throw new NotImplementedException();
            }
        }

        internal void Purge()
        {
            throw new NotImplementedException();
        }

        public Status FindStatus(Aura aura)
        {
            foreach (Status status in _statuses)
            {
                if (status.Aura == aura)
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
                if (status.Aura.Id == spell)
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

        internal void TakeDamage(DamageEvent @event)
        {
            for (int i = 0; i < _statuses.Count; i++)
            {
                _statuses[i].TakeDamage(@event);
            }
        }

        internal void AbsorpDamage(DamageEvent @event)
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

        internal void AbsorbDamage(CastInputData data, float absorption, SchoolType school)
        {
            //Status status = FindStatus(data.Spell.Id) ?? AddStatus(data.Target, data.Spell);
            //status.TryApplyCaster(data.Caster);
            //status.GiveAbsorption(new(absorption, school));
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

        public void Update(IActionRecordContainer container)
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

        internal void InformAction(UnitAction action, CastInputData data)
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
            Status result = new Status(target, aura);
            _statuses.Add(result);
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