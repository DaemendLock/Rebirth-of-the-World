using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    public class CharacterModel : MonoBehaviour
    {
        [SerializeField] private string _modelName;

        public Quaternion LookDirection { get; set; }

        public EntityId Id { get; set; }

        public ModelName ModelName { get => new(_modelName); set => _modelName = value.Value; }

        public float TimeScale { get; set; }
    }
}
