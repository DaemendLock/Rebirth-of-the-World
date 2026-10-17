using Combat.Common.Primitives;
using Combat.Local.Application.ValueObjects;

using System.Collections.Generic;

namespace Combat.Domain.Entities
{
    public readonly struct SkillOwner
    {
        private readonly List<SkillCooldown> _cooldowns;
        private readonly List<SkillId> _skills;

        public SkillOwner(UnitId id)
        {
            Id = id;
            _skills = new();
            _cooldowns = new();
        }

        public UnitId Id { get; }

        public IReadOnlyCollection<SkillId> Skills => _skills;

        public void GiveSkill(SkillId skillId)
        {
            if (_skills.Contains(skillId))
            {
                throw new System.InvalidOperationException("Skill is already given.");
            }

            _skills.Add(skillId);
        }

        public bool RemoveSkill(SkillId skillId) => _skills.Remove(skillId);

        public SkillId? GetSkill(int index)
        {
            if (_skills.Count <= index || index < 0)
                return null;

            return _skills[index];
        }

        public bool HasSkill(SkillId value)
        {
            foreach (var skill in _skills)
            {
                if (skill == value)
                {
                    return true;
                }
            }

            return false;
        }

        public float GetCooldown(SkillId skillId)
        {
            foreach (var value in _cooldowns)
            {
                if (value.Skill != skillId)
                {
                    continue;
                }

                return value.Value;
            }

            return 0f;
        }

        public void SetCooldown(SkillId skillId, float value)
        {
            for (int i = 0; i < _cooldowns.Count; i++)
            {
                if (_cooldowns[i].Skill != skillId)
                {
                    continue;
                }

                _cooldowns[i] = new(skillId, value);
                return;
            }

            _cooldowns.Add(new(skillId, value));
        }

        public void ProgressCooldowns(float deltaTime)
        {
            for (int i = 0; i < _cooldowns.Count; i++)
            {
                _cooldowns[i] = _cooldowns[i].Progress(deltaTime);
            }

            _cooldowns.RemoveAll(value => value.Value <= 0);
        }
    }
}
