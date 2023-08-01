using Remaster.AuraEffects;
using Remaster.Events;
using Remaster.Interfaces;
using Remaster.Stats;
using Remaster.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Remaster.UnitComponents
{
    public class AuraOwner : DynamicStatOwner
    {
        private Dictionary<Spell, Status> _statuses = new Dictionary<Spell, Status>();
        private Dictionary<Mechanic, int> _immunes = new Dictionary<Mechanic, int>();

        public Status AddStatus(Unit target, Spell spell)
        {
            Status result = new Status(target, spell);
            _statuses[spell] = result;

            return result;
        }

        public void ApplyAura(EventData data, AuraEffect effect)
        {
            Status status = FindStatus(data.Spell) ?? AddStatus(data.Target, data.Spell);
            status.TryApplyCaster(data.Caster);
            status.AddEffect(effect);
        }

        public void RemoveStatus(Spell spell)
        {
            Status removableStatus = FindStatus(spell);

            if (removableStatus == null)
            {
                return;
            }

            removableStatus.ClearEffects();
            _statuses.Remove(spell);
        }

        public bool HasStatus(Spell spell) => FindStatus(spell) != null;

        public void Dispell(DispellType type)
        {
            foreach (Status statuses in _statuses.Values)
            {
                throw new NotImplementedException();
            }
        }

        public void Purge()
        {
            throw new NotImplementedException();
        }

        public Status FindStatus(Spell spell)
        {
            if (_statuses.ContainsKey(spell) == false)
            {
                return null;
            }

            return _statuses[spell];
        }

        public PercentModifiedValue EvaluateStat(UnitStat stat)
        {
            if (stat == UnitStat.MAX_HEALTH)
            {
                return new PercentModifiedValue();
            }

            PercentModifiedValue result = new PercentModifiedValue();

            foreach (Status status in _statuses.Values)
            {
                result += status.EvaluateStat(stat);
            }

            return result;
        }

        public void Update()
        {
            IEnumerable<KeyValuePair<Spell, Status>> list = _statuses.Where((pair) => pair.Value.Expired);

            foreach (KeyValuePair<Spell, Status> pair in list)
            {
                _statuses.Remove(pair.Key);
            }
        }

        public void InformAction(UnitAction action, EventData data)
        {
            foreach (Status status in _statuses.Values)
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
    }
}