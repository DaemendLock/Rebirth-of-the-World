using Combat.Common.ValueObjects;
using Combat.Local.Controllers;
using Combat.Local.Data.Databases;
using Combat.Local.Data.Entities;

using Temp.Domain.Implementations;

using UnityEngine;

namespace Testing.Local
{
    public class UnitProperties : MonoBehaviour
    {
        [Zenject.Inject] private CombatController _combatController;
        [Zenject.Inject] private SkillDataBase _skillDb;

        [SerializeField] private string _modelName;

        [field: SerializeField] public int UnitId { get; private set; }
        [field: SerializeField] public byte Team { get; private set; }
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public float InitialHealth { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public CastableSkillData[] Skills { get; private set; }
        public ModelName ModelName => new(_modelName);

        private void Awake()
        {
            foreach (CastableSkillData value in Skills)
            {
                _skillDb.Load(value);
            }
        }

        private void Start()
        {
            _combatController.CreateUnit(GetUnitCreationData(), transform);
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

            return new(ModelName, new(Team), transform.position, MaxHealth, InitialHealth, statsTable.ToAttributeArray(), skills);
        }
    }
}
