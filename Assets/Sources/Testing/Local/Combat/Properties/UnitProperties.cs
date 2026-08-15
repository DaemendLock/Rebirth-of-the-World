using Combat.Common.ValueObjects;
using Combat.Local.Controllers;
using Combat.Local.Data.Databases;
using Combat.Local.Data.DataSources;
using Combat.Local.Domain.ValueObjects;

using Data.Entities;

using System.Linq;

using Temp.Domain.Implementations;

using UnityEngine;

namespace Testing.Local
{
    [System.Serializable]
    public class ResourceTemplate
    {
        public int ResourceId;
        public float BaseValue;
        public float MaxValue;

        public ResourceValue ToResourceValue => new(new(ResourceId), BaseValue, MaxValue);
    }

    public sealed class UnitProperties : MonoBehaviour
    {
        [Zenject.Inject] private EncounterController _combatController;
        [Zenject.Inject] private SceneObjectDataSource _sceneObjectlDataSource;
        [Zenject.Inject] private SkillDataBase _skillDb;

        [SerializeField] private int _targetId;
        [SerializeField] private string _modelName;

        [field: SerializeField] public int UnitId { get; private set; }
        [field: SerializeField] public byte Team { get; private set; }
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public float InitialHealth { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public SkillData[] Skills { get; private set; }
        [field: SerializeField] public ResourceTemplate[] Resources { get; private set; }
        public ModelName ModelName => new(_modelName);

        private void Awake()
        {
            foreach (SkillData value in Skills)
            {
                _skillDb.Load(value);
            }
        }

        private void Start()
        {
            _sceneObjectlDataSource.Register(new(_targetId), transform);
            _combatController.CreateUnit(new(_targetId), GetUnitCreationData());
            Destroy(this);
        }

        private UnitCreationInfo GetUnitCreationData()
        {
            StatsTable statsTable = StatsTable.UnitDefault;
            statsTable[Attribute.Speed] = new(MoveSpeed, 100);
            statsTable[Attribute.Haste] = new(0, 100);

            SkillId[] skills = new SkillId[Skills.Length];

            for (int i = 0; i < skills.Length; i++)
            {
                if (Skills[i] == null)
                {
                    continue;
                }

                skills[i] = Skills[i].Id;

                if (skills[i] == null)
                    Debug.Log("Null skill");
            }

            return new(ModelName, new(Team), transform.position, MaxHealth, InitialHealth, statsTable.ToAttributeArray(), Resources.Select(value => value.ToResourceValue).ToArray(), skills);
        }
    }
}
