using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.OutputPorts
{
    public interface IObjectiveCreateHandler
    {
        void Create(Objective objective);
    }
}
