using Combat.Common.Primitives;

using Data.Entities.NpcModels;

using UnityEngine;

namespace Data.Entities
{
    public class CharacterModel : MonoBehaviour
    {
        [SerializeField] private Hardpoint[] _hardpoints;

        private void Awake()
        {
            _hardpoints = GetComponentsInChildren<Hardpoint>();
        }

        public GameObject Prefab => gameObject;
        public ModelName Name => new(name);
    }
}
