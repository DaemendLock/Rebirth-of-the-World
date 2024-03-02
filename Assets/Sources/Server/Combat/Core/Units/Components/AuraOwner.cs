using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine;
using Core.Combat.Statuses;
using Core.Combat.Statuses.Auras;
using Core.Combat.Units.Interfaces;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using System;
using System.Collections.Generic;
using Utils.DataStructure;
using Utils.DataStructures;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public sealed class AuraOwner : Updatable, StatsOwner
    {
        private List<Status> _statuses = new List<Status>();
        private ArrayQueue<int> _delete = new(16);

        internal void ApplyAura(CastInputData data, Aura aura, long duration)
        {
            Status status = FindStatus(aura);

            if (aura.Flags.HasFlag(AuraFlags.SeparateStatuses) || status == null)
            {
                status = AddStatus(data.Target, aura, duration);
            }

            status.TryApplyCaster(data.Caster);
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

        public Status FindStatus(Aura aura)
        {
            throw new NotImplementedException();
        }

        public Status FindStatus(SpellId spell)
        {
            throw new NotImplementedException();
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

        public int EvaluateStatValue(UnitStat stat) => (int) EvaluateStat(stat).CalculatedValue;

        internal void AmplifyOutcomeDamage(HealthChangeEvent @event)
        {
            foreach (Status status in _statuses)
            {
                status.AmplifyOutcomeDamage(@event);
            }
        }

        internal void AmplifyOutcomeHealing(HealthChangeEvent @event)
        {

        }

        internal void OnDealDamage(HealthModificationRecord record)
        {

        }

        internal void OnHealing(HealthModificationRecord record)
        {

        }

        internal void ApplyIncomeDamageModification(HealthChangeEvent @event)
        {
            for (int i = 0; i < _statuses.Count; i++)
            {
                throw new NotImplementedException();
                // _statuses[i].TakeDamage(@event);
            }
        }

        internal void OnTakeDamage(HealthModificationRecord record)
        {

        }

        public void Update(IActionRecordContainer container)
        {
            lock (this)
            {
                for (int i = 0; i < _statuses.Count; i++)
                {
                    if (_statuses[i].Expired)
                    {
                        container.AddAction(_statuses[i].ClearEffects());
                        MarkUpDelete(i);
                        continue;
                    }

                    _statuses[i].Update(container);
                }

                Flush();
            }
        }

        private Status AddStatus(Unit target, Aura aura, long duration)
        {
            Status result = new Status(target, aura, duration);
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