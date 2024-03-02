using Client.Combat.View.UI.Nameplates;
using UnityEngine;
using Utils.DataTypes;

namespace Client.Combat.View.Units
{
    public static class UnitFactory
    {
        public static Unit CreateUnit(int id, UnitCreationData data)
        {
            GameObject unit = new("Unit" + id);

            Unit result = unit.AddComponent<Unit>();
            result.Init(id, data.Veiw);
            //TODO: NameplatesRoot.CreateNameplate(id);

            return result;
        }
    }
}
