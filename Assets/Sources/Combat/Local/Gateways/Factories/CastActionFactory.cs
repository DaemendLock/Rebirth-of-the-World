using CastStateSkill;

using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.ValueObjects;
using Combat.Local.Application.Factories;
using Combat.Local.Gateways.DataSources;

namespace Combat.Local.Gateways.Factories
{
    public sealed class CastActionFactory : ICastActionFactory
    {
        private readonly ISkillDataBase _skillDataBase;

        public CastActionFactory(ISkillDataBase skillDataBase)
        {
            _skillDataBase = skillDataBase;
        }

        public CastActivity Create(ActionId actionId, SkillId skillId, SkillFlags skillFlags, ActionFlags flags)
        {
            if (_skillDataBase.TryGetFrameData(actionId, out IFrameData frameData) == false)
            {
                throw new System.InvalidOperationException();
            }

            CastActivity castActivity = new(skillId, frameData);

            if (flags.HasFlag(ActionFlags.Holdable) == false)
            {
                castActivity.Release();
            }

            return castActivity;
        }
    }
}
