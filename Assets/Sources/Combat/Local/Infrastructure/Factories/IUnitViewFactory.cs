using Combat.Common.ValueObjects;
using Combat.Local.Infrastructure.Presenters;

using UnityEngine;

namespace Combat.Local.Factories
{
    public interface IUnitViewFactory
    {
        public readonly ref struct UnitViewCreationInfo
        {
            public readonly EntityId EntityId;
            public readonly ModelName ModelName;
            public readonly Transform Parent;
            public readonly Vector3 Position;

            public UnitViewCreationInfo(EntityId entityId, ModelName modelId, Transform parent, Vector3 position)
            {
                ModelName = modelId;
                EntityId = entityId;
                Parent = parent;
                Position = position;
            }
        }

        IUnitPresenter Create(UnitViewCreationInfo context);
    }
}
