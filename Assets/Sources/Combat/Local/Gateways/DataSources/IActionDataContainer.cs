using Combat.Common.ValueObjects;
using Combat.Local.Data.Models;

namespace Combat.Local.Gateways.DataSources
{
    public interface IActionDataContainer
    {
        void Register(ActionId id, ActionData actionData);
        ActionData Get(ActionId id);
    }
}
