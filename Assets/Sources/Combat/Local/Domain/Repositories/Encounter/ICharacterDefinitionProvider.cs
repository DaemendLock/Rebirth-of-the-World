using Combat.Common.Primitives;
using Combat.Local.Domain.UseCases.Encounter;

namespace Combat.Local.Domain.Repositories
{
    public interface ICharacterDefinitionProvider
    {
        CharacterDefinition Get(CharacterKey key);
    }
}
