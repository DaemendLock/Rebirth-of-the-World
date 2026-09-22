using Combat.Common.Primitives;

namespace Game.Domain.ValueObjects
{
    public readonly struct CharacterSelection
    {
        public CharacterSelection(CharacterKey characterKey)
        {
            CharacterKey = characterKey;
        }

        public CharacterKey CharacterKey { get; }
    }
}
