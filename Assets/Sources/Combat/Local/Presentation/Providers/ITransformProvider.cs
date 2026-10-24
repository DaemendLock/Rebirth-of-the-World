using Combat.Common.Primitives;

using UnityEngine;

namespace Combat.Local.Presentation.Providers
{
    public interface ITransformProvider
    {
        bool TryGet(UnitId unitId, out Transform transform);
    }
}
