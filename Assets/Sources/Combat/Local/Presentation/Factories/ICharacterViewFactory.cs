using Combat.Common.ValueObjects;
using Combat.Local.Presentation.Components;

namespace Combat.Local.Presentation.Factories
{
    public interface ICharacterViewFactory
    {
        void Init(CharacterView target);
    }
}
