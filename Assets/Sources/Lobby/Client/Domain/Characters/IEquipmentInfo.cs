using Client.Lobby.Domain.Items;

namespace Client.Lobby.Domain.Characters
{
    public interface IEquipmentInfo
    {
        Item GetItem(GearSlot slot);
    }
}
