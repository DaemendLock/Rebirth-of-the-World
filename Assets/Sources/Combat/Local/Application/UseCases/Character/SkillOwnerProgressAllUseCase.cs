using Combat.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Application.UseCases
{

    public sealed class SkillOwnerProgressAllUseCase
    {
        private readonly IUnitStore _unitStore;

        public SkillOwnerProgressAllUseCase(IUnitStore unitStore)
        {
            _unitStore = unitStore;
        }

        public void Execute(ReadOnlySpan<Updatable> targets, float progressTime)
        {
            foreach (Updatable target in targets)
            {
                _unitStore.Get(target.Id).Skills.ProgressCooldowns(progressTime * target.TimeScale);
            }
        }
    }
}
