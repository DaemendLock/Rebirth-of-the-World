using Combat.API.Contexts;
using Combat.Common.Primitives;

namespace Combat.API.Events
{
    public readonly struct ObjectiveCompletedEventData : IEventData
    {
        public readonly ObjectiveId ObjectiveId;

        public ObjectiveCompletedEventData(ObjectiveId objectiveId)
        {
            ObjectiveId = objectiveId;
        }
    }
}
