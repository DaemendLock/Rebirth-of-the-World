using Combat.Local.Domain.API;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.ValueObjects;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("DpsPaladin3Buff")]
    public class DpsPaladin3Buff : StatusApi, IResourceGainSpendHandler
    {
        public void OnGainResource(ResourceChangeRecord @event) { }

        public void OnSpendResource(ResourceChangeRecord @event)
        {
            Duration duration = Duration;
            duration.FullDuration += @event.Value * 0.05f;
        }
    }
}
