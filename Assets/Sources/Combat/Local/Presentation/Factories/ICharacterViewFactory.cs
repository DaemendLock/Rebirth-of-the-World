using Combat.Common.ValueObjects;
using Combat.Local.Presentation.Components;

namespace Combat.Local.Presentation.Factories
{
    public interface ICharacterViewFactory
    {
        CharacterView Create(EntityId id, ModelName modelName);
        void Init(EntityId id, CharacterView target);
    }
}
