using Remaster.AuraEffects;
using Remaster.Events;
using Remaster.Interfaces;
using Remaster.Stats;
using Remaster.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Remaster
{
    public class Status : DynamicStatOwner
    {
        public readonly Unit Caster;
        public readonly Unit Parent;
        public readonly Spell Spell;

        private Duration _duration;

        private LinkedList<AuraEffect> _effects;
        private LinkedList<DynamicModStat> _dynamicStat;

        private Dictionary<UnitAction, List<DynamicEffect>> _dynamicEffects;

        public Status(EventData data)
        {
            _effects = new LinkedList<AuraEffect>();
            _dynamicEffects = new Dictionary<UnitAction, List<DynamicEffect>>();
            _dynamicStat = new LinkedList<DynamicModStat>();

            Caster = data.Caster;
            Parent = data.Target;
            Spell = data.Spell;
            _duration = new Duration(data.Spell.Duration);

            Logger.Log($"Status({Spell.Id}) applied: {Caster} -> {Parent}, {_duration.FullTime} sec.");
        }

        public int StackCount { get; private set; }

        public bool Expired => _duration.Expired;

        public void AddEffect(AuraEffect effect)
        {
            effect.ApplyEffect(this);
        }

        public void Refresh(EventData data)
        {
            _duration += data.Spell.Duration - _duration.Left;
        }

        public void SetDuration(float duration)
        {
            _duration += duration - _duration.Left;
        }

        public void Remove()
        {
            Parent.RemoveStatus(Spell, Caster);
        }

        public void RegisterDynamicEffect(DynamicEffect effect, UnitAction action)
        {
            if (_dynamicEffects.ContainsKey(action) == false)
            {
                _dynamicEffects[action] = new List<DynamicEffect>();
            }

            _dynamicEffects[action].Add(effect);
        }

        public void RegisterDynamicStat(DynamicModStat effect)
        {
            if (_dynamicStat.Contains(effect) == false)
            {
                _dynamicStat.AddLast(effect);
            }
        }

        public void ClearEffects()
        {
            foreach (AuraEffect effect in _effects)
            {
                effect.RemoveEffect(this);
            }

            _dynamicEffects = null;
            _dynamicStat = null;

            Logger.Log($"Status({Spell.Id}) effects cleard: {Parent}");
        }

        public PercentModifiedValue EvaluateDynamicStat(UnitStat stat)
        {
            PercentModifiedValue result = new PercentModifiedValue();

            foreach (DynamicModStat effect in _dynamicStat.Where((effect) => effect.Stat == stat))
            {
                result += effect.Evaluate(this);
            }

            return result;
        }

        public void CallAction(UnitAction action, EventData data)
        {
            if (_dynamicEffects.ContainsKey(action) == false)
            {
                return;
            }

            foreach (DynamicEffect effect in _dynamicEffects[action])
            {
                effect.Update(this, data);
            }
        }
    }

    public enum UnitAction
    {
        TAKE_DAMAGE,
        DEAL_DAMAGE,
        RESOTORE_HEALTH,
        UPDATE_RESOURCES_VALUE,
        AUTOATTACK,
    }
}
