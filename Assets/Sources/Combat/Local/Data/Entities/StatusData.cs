using UnityEngine;
using UnityEngine.UI;

namespace Combat.Local.Data.Entities
{
    [CreateAssetMenu(menuName = "Assets/Skills/Status")]
    public class StatusData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string ScriptName { get; private set; }
        [field: SerializeField] public Image Icon { get; private set; }
    }
}
