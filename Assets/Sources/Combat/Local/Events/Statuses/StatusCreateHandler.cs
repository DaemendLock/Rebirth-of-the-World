using Combat.Common.ValueObjects;

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
}
