using Combat.Common.Primitives;
using Combat.Local.Gateways.Models;

namespace Combat.Local.Gateways.DataSources
{
    public interface ICombatCharacterDataBase
    {
        bool TryGet(CharacterKey key, out CharacterDefinitionModel value);
    }
}
