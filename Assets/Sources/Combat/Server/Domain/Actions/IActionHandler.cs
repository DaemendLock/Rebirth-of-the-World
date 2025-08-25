using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.Actions
{
    public interface IActionHandler
    {
        Unit Actor { get; }

        bool IsActive { get; }
        float ActiveTime { get; set; }
    }
}
