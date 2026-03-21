using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Statuses;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("DpsPaladin3Buff")]
    public class DpsPaladin3Buff : CustomStatusStrategy, IResourceGainSpendHandler
    {
        public void OnGainResource(ResourceChangeRecord @event) { }

        public void OnSpendResource(ResourceChangeRecord @event)
        {
            if (@event.Resource == Combat.Common.ValueObjects.ResourceId.Custom)
            {
                Instance.ExtendDuration(@event.Value * 0.05f);
            }
        }
    }
}
