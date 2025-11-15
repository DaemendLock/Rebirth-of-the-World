using Combat.Common.ValueObjects;

using System;

namespace Combat.Local.Domain.Entities.Player
{
    public readonly ref struct SkillPanel
    {
        private readonly Span<SkillId?> _skills;

        public SkillPanel(Span<SkillId?> skills)
        {
            _skills = skills;
        }

        public int Count => _skills.Length;

        public SkillId? this[int index]
        {
            get => index >= Count ? null : _skills[index];
            set
            {
                if (index >= Count)
                {
                    return;
                }

                _skills[index] = value;
            }
        }
    }
}
