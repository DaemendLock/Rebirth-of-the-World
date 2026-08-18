using Combat.Common.Primitives;

namespace Combat.Local.Scripting.Runtime
{
    public interface IUnitRuntimeRegistry
    {
        void Create(UnitNew unitNew);
        void Remove(UnitId id);
        bool TryGet(UnitId id, out UnitNew unitNew);

    }
}
