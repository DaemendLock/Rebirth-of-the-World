using Data.Entities;

namespace Data.Skills.Components
{
    [UnityEngine.RequireComponent(typeof(SkillData))]
    public class SkillScriptComponent : UnityEngine.MonoBehaviour, ISkillComponent
    {
        [field: UnityEngine.SerializeField] public string ScriptName { get; private set; }
    }

    public class SkillRequireTargetComponent
    {
        [field: UnityEngine.SerializeField] public float MaxRange { get; private set; }
    }
}
