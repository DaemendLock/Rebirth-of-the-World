using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Events
{
    public readonly ref struct StatusCreateInfo
    {
        public StatusCreateInfo(StatusId statusId, EntityId parentId, StatusName statusName, SkillId? source, EntityId? caster)
        {
            StatusId = statusId;
            ParentId = parentId;
            StatusName = statusName;
            Source = source;
            Caster = caster;
        }

        public StatusId StatusId { get; }

        public EntityId ParentId { get; }

        public StatusName StatusName { get; }

        public SkillId? Source { get; }

        public EntityId? Caster { get; }
    }

    public class StatusCreateHandler : IApplyStatusEventHandler
    {
        public delegate void Handle(StatusCreateInfo info);

        public StatusCreateHandler()
        {
        }

        public event Handle Created;

        public void HandleEvent(Status status)
        {
            StatusCreateInfo info = new(status.Id, status.Parent, status.Name, status.Source, status.Caster);
            Created?.Invoke(info);
        }
    }
}
