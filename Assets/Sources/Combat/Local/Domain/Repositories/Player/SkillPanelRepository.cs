using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Player;

namespace Combat.Local.Domain.Repositories.Player
{
    public interface ISkillPanelRepository
    {
        void Create(EntityId id, SkillPanel skillPanel);
        void Update(EntityId id, SkillPanel skillPanel);
        SkillPanel Get(EntityId id);
        void Delete(EntityId id);
    }
}
