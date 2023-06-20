using Remaster.AuraEffects;
using Remaster.Events;
using Remaster.Interfaces;
using Remaster.Stats;
using Remaster.Utils;
using System;
using System.Collections.Generic;

namespace Remaster.UnitComponents
{
    public class AuraOwner : DynamicStatOwner
    {
        private Dictionary<Spell, List<Status>> _statuses = new Dictionary<Spell, List<Status>>();
        private Dictionary<Mechanic, int> _immunes = new Dictionary<Mechanic, int>();

        public Status AddStatus(EventData data)
        {
            Spell spell = data.Spell;

            if (_statuses.ContainsKey(spell) == false)
            {
                _statuses[spell] = new List<Status>();
            }

            Status result = new Status(data);
            _statuses[spell].Add(result);

            return result;
        }

        public void ApplyAura(EventData data, AuraEffect effect)
        {
            Status status = FindStatus(data.Spell, data.Caster);

            if (status == null)
            {
                status = AddStatus(data);
            }

            status.AddEffect(effect);
        }

        public void RemoveStatus(Spell spell, Unit caster)
        {
            Status removableStatus = FindStatus(spell, caster);

            if (removableStatus == null)
            {
                return;
            }

            removableStatus.ClearEffects();
            _statuses[spell].Remove(removableStatus);
        }

        public bool HasStatus(Spell spell, Unit caster) => FindStatus(spell, caster) != null;

        public void Dispell(DispellType type)
        {
            foreach (List<Status> statuslist in _statuses.Values)
            {
                throw new NotImplementedException();
            }
        }

        public void Purge()
        {
            throw new NotImplementedException();
        }

        public Status FindStatus(Spell spell, Unit caster)
        {
            if (_statuses.ContainsKey(spell) == false)
            {
                return null;
            }

            return _statuses[spell].Find((status) => status.Caster == caster);
        }

        public PercentModifiedValue EvaluateDynamicStat(UnitStat stat)
        {
            if (stat == UnitStat.MAX_HEALTH)
            {
                return new PercentModifiedValue();
            }

            PercentModifiedValue result = new PercentModifiedValue();

            foreach (List<Status> list in _statuses.Values)
            {
                foreach (Status status in list)
                {
                    result += status.EvaluateDynamicStat(stat);
                }
            }

            return result;
        }

        public void Update()
        {
            int i;

            foreach (List<Status> list in _statuses.Values)
            {
                for (i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i].Expired)
                    {
                        list[i].ClearEffects();
                        list.RemoveAt(i);
                    }
                }
            }
        }

        public void InformAction(UnitAction action, EventData data)
        {
            foreach (List<Status> list in _statuses.Values)
            {
                foreach (Status status in list)
                {
                    status.CallAction(action, data);
                }
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
    }
}