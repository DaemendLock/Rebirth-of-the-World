using Combat.Common.ValueObjects;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Flags;

using JetBrains.Annotations;

namespace Combat.Local.Domain.API
{
    public class ScriptedSkill
    {
        private Skill _data;

        public SkillId Id => _data.Id;

        public Scene Enviroment { get; private set; }

        public Unit Caster { get; private set; }

        public bool IsWeaponAttack => _data.Flags.HasFlag(SkillFlags.WeaponAttack);

        public virtual void OnCast() { }

        /// <summary>
        /// Called when casting skill to determine if cast is possible.
        /// </summary>
        /// <returns>True if cast is possible.</returns>
        public virtual bool CanCast() => true;

        public bool TryGetProperty<T>(out T property)
        {
            if (this is not T value)
            {
                property = default;
                return false;
            }

            property = value;
            return true;
        }

        protected float GetCooldownRemain() => Caster.GetCooldown(Id);

        protected virtual void OnInit() { }

        [UsedImplicitly()]
        private void Init(ScriptedSkillContext context)
        {
            _data = context.Skill;
            Caster = context.Caster;
            Enviroment = context.Enviroment;
            OnInit();
        }
    }
}
