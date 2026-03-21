using Data.Entities;

namespace Data.Skills.Components
{
    [UnityEngine.RequireComponent(typeof(SkillData))]
    public class SkillActionsComponent : UnityEngine.MonoBehaviour, ISkillComponent
    {
        [UnityEngine.SerializeField] private ActionData[] _values;

        public ActionData[] Values => _values;
    }
}
