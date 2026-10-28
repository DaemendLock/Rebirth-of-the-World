using Combat.Domain.Entities;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class SkillOwnerProgressAllUseCase
    {
        public void Execute(ReadOnlySpan<UnitContext> targets, float progressTime)
        {
            foreach (UnitContext target in targets)
            {
                target.Skills.ProgressCooldowns(progressTime * target.Attributes.TimeScale);
            }
        }
    }
}
