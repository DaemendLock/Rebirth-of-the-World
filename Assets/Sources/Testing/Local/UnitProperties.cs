using Combat.Common.ValueObjects;
using Combat.Local.Data.Entities;
using Combat.Local.Data.Services;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Factories;
using Combat.Local.Infrastructure.Controllers;
using Combat.Local.Presentation.Components;

using Temp.Domain.Implementations;

using UnityEngine;

namespace Testing.Local
{
    public class UnitProperties : MonoBehaviour
    {
        [Zenject.Inject] private CombatController _combatController;
        [Zenject.Inject] private ISkillRegistrationService _skillDataRepository;

        [field: SerializeField] public int UnitId { get; private set; }
        [field: SerializeField] public byte Team { get; private set; }
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public float InitialHealth { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public CastableSkillData[] Skills { get; private set; }

        private void Awake()
        {
            foreach (CastableSkillData skill in Skills)
            {
                _skillDataRepository.Register(skill);
            }
        }

        private void Start()
        {
            _combatController.CreateUnit(GetUnitCreationData());
            Destroy(this);
        }

        private IUnitControllerFactory.UnitModelCreationData GetUnitCreationData()
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

            CharacterView characterView = GetComponent<CharacterView>() ?? GetComponentInChildren<CharacterView>();
            ModelName name = characterView.UnitName;

            return new(name, Team, transform, transform.position, MaxHealth, InitialHealth, statsTable, skills);
        }
    }
}
