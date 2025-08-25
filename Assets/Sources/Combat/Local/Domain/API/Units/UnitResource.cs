using System.Collections.Generic;
using System.Linq;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.API
{
    public sealed partial class Unit
    {
        public float GetResourceValue(ResourceId resource) => _giveSpendResourceService.GetResourceValue(Id, resource);

        public float GiveResource(ScriptedSkill source, ResourceId resource, float value)
        {
            Resource result = _giveSpendResourceService.GiveResource(Id, resource, value);
            IEnumerable<StatusApi> effects = _statusLookupService.FindStatusesOnUnit(Id);

            foreach (StatusApi status in effects)
            {
                if (status is not IResourceGainSpendHandler handler)
                {
                    continue;
                }

                handler.OnGainResource(new(resource, source, value));
            }

            return result.CurrentValue;
        }

        public float SpendResource(ScriptedSkill source, ResourceId resource, float value)
        {
            Resource result = _giveSpendResourceService.SpendResource(Id, resource, value);
            IEnumerable<StatusApi> effects = _statusLookupService.FindStatusesOnUnit(Id);

            foreach (StatusApi status in effects)
            {
                if (status is not IResourceGainSpendHandler handler)
                {
                    continue;
                }

                handler.OnSpendResource(new(resource, source, value));
            }

            return result.CurrentValue;
        }
    }
}
