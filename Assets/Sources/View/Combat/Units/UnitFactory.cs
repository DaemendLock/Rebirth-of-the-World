using Data.Model;
using UnityEngine;
using Utils.DataTypes;
using View.Combat.UI.Nameplates;

namespace View.Combat.Units
{
    public static class UnitFactory
    {
        public static void CreateUnit(UnitCreationData data)
        {
            int id = data.Id;

            GameObject unit = new("Unit" + id);

            Unit result = unit.AddComponent<Unit>();
            result.Init(id, data.Veiw);
            NameplatesRoot.CreateNameplate(id);
        }
    }
}
