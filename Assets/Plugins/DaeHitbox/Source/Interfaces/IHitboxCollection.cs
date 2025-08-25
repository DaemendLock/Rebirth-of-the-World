using System.Collections.Generic;

namespace DaeHitbox
{
    public interface IHitboxCollection : IEnumerable<IHitbox>
    {
        IHitbox GetHitbox(HitboxType type);
    }
}