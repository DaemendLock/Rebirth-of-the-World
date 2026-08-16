using Lobby.Local.Domain.Entities.Characters;

namespace Lobby.Local.Domain.ValueObjects
{
    public readonly struct Level
    {
        public readonly int CurrentValue;
        public readonly int MaxValue;
        public readonly int CurrentProgress;
        public readonly int TargetProgress;

        public Level(int currentValue, int maxValue, int currentProgress, int targetProgress)
        {
            CurrentValue = currentValue;
            MaxValue = maxValue;
            CurrentProgress = currentProgress;
            TargetProgress = targetProgress;
        }
    }

    public readonly struct CharacterFilter
    {
        public bool IsOwned { get; }

        public bool Validate(CharacterProgression character)
        {
            if (IsOwned && (character.IsAvailable == false))
            {
                return false;
            }

            return true;
        }
    }
}
