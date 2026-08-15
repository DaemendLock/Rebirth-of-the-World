using Lobby.Common.Primitives;
using Lobby.Local.Domain.ValueObjects;

namespace Lobby.Local.Domain.UseCases
{
    public readonly struct CharacterInfo
    {
        public CharacterId ChracterId { get; }
        public string Name { get; }
        public Level Level { get; }
    }

    public interface ICharacterInfoRepository
    {
        CharacterInfo Get(CharacterId characterId, AccountId accountId);
    }

    public sealed class GetCharacterInfoUseCase
    {
        private readonly ICharacterInfoRepository _characterInfoRepository;

        public CharacterInfo Execute(CharacterId characterId, AccountId accountId)
        {
            return _characterInfoRepository.Get(characterId, accountId);
        }
    }
}
