using Combat.API;
using Combat.API.API.Skills;
using Combat.API.Objectives;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories.Skill;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Capabilities.Skills;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Idk
{
    public sealed class RuntimeObjectiveContainer
    {
        private readonly ICombatObjective _combatObjective;
        private readonly IObjectiveContext _context;

        public RuntimeObjectiveContainer(ICombatObjective combatObjective, IObjectiveContext context)
        {
            _combatObjective = combatObjective;
            _context = context;
        }

        public ICombatObjective CombatObjective => _combatObjective;

        public IObjectiveContext Context => _context;
    }

    public sealed class NewScriptAbilityPropertyContainer : IAbilityPropertyContainer
    {
        private readonly ICastableSkill _castableSkill;

        public NewScriptAbilityPropertyContainer(AbilityKey id, ISkillScriptNew script, UnitNewAdapter unitNewAdapter, ISkillMemoryRepository skillMemoryRepository)
        {
            UnitNew unitNew = null;

            if (id.Owner.HasValue)
            {
                unitNew = unitNewAdapter.Adaptee(id.Owner.Value);
            }

            _castableSkill = new NewCast(id, skillMemoryRepository, script, unitNew);
        }

        public bool TryGet(out HandleSkillHitCapability result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out HandleSkillActionStateChangeCapability result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out EvaluateSkillTargetCapability result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out ExecuteSkillCapability result)
        {
            result = new(_castableSkill);
            return true;
        }

        private sealed class NewCast : ICastableSkill
        {
            private readonly AbilityKey _abilityKey;
            private readonly ISkillMemoryRepository _memoryRepository;
            private readonly ISkillScriptNew _skillScript;
            private readonly UnitNew _unitNew;

            public NewCast(AbilityKey abilityKey, ISkillMemoryRepository memoryRepository, ISkillScriptNew skillScript, UnitNew unitNew)
            {
                _abilityKey = abilityKey;
                _memoryRepository = memoryRepository;
                _skillScript = skillScript;
                _unitNew = unitNew;
            }

            public bool OnCast()
                => _skillScript.OnCast(_unitNew, new DomainSkillContext(_memoryRepository, _abilityKey));
        }
    }
}
