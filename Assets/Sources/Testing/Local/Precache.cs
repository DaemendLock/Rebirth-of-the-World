using Combat.Local.Data.Entities;
using Combat.Local.Data.Services;

using UnityEngine;

namespace Testing.Local
{
    public class Precache : MonoBehaviour
    {
        [Zenject.Inject] private ISkillRegistrationService _skillDataRepository;

        private void Awake()
        {
            foreach (CastableSkillData skill in Skills)
            {
                _skillDataRepository.Register(skill);
            }
        }

        [field: SerializeField] public CastableSkillData[] Skills { get; private set; }
    }
}
