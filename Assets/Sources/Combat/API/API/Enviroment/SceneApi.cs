using Combat.API.DTO;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Controllers;

namespace Combat.API
{
    public sealed class SceneApi
    {
        private readonly CombatController _combatController;

        public SceneApi(CombatController combatController)
        {
            _combatController = combatController;
        }

        public object CreateProjectile(object from, object speed, IHitHandler hitHandler)
        {
            throw new System.NotImplementedException();
        }

        public void CreateUnit(UnitCreationData data)
        {
            UnitCreationInfo values = new(data.ModelName, data.Team, data.Position, data.BaseHealth, -1, data.Attributes, System.Array.Empty<SkillId>());
            _combatController.CreateUnit(values);
        }

        public void CreateHitHandler(Unit hitSource, IHitHandler hitHandler)
        {

        }
    }
}
