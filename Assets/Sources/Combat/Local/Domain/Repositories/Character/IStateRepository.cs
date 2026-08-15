using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Repositories
{
    public interface IMoveOverTimeEffectRepository
    {
        void Create(MoveInDirectionEffect value);
        void Update(MoveInDirectionEffect value);
        MoveInDirectionEffect Get(TransformEffectId id);
        void Delete(TransformEffectId id);
    }
}
