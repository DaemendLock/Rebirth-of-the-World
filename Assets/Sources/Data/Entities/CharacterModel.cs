using Combat.Common.ValueObjects;

using UnityEngine;

namespace Data.Entities
{
    [CreateAssetMenu(menuName = "Assets/Characters/Model")]
    public class CharacterModel : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;

        public GameObject Prefab => _prefab;
        public ModelName Name => new(name);
    }
}
