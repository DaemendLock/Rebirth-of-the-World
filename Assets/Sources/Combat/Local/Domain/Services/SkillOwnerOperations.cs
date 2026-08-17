using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Characters;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.Services.Skills
{
    public sealed class ItemOwnerOperations
    {
        private readonly IItemOwnerRepository _repository;

        public void Equip(ItemOwner itemOwner, ItemSlot itemInfo, EquipmentSlotType slot)
        {
            ItemSlot?[] items = new ItemSlot?[itemOwner.Items.Length];
            itemOwner.Items.CopyTo(items);
            items[(int)slot] = itemInfo;
            _repository.Update(new(itemOwner.Id, items));
        }
    }

    public sealed class SkillOwnerOperations
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;

        public SkillOwnerOperations(ISkillOwnerRepository skillOwnerRepository)
        {
            _skillOwnerRepository = skillOwnerRepository;
        }

        public void SetCooldown(SkillOwner skillOwner, SkillId skill, float cooldown)
        {
            Span<SkillCooldown> skillCooldowns = stackalloc SkillCooldown[skillOwner.Cooldowns.Length + 1];
            skillOwner.Cooldowns.CopyTo(skillCooldowns);

            for (int i = 0; i < skillCooldowns.Length - 1; i++)
            {
                if (skillCooldowns[i].Skill != skill)
                {
                    continue;
                }

                skillCooldowns[i] = new(skillCooldowns[i].Skill, cooldown);
                _skillOwnerRepository.Update(new(skillOwner.Id, skillOwner.Skills, skillCooldowns[..^1]));
                return;
            }

            skillCooldowns[^1] = new(skill, cooldown);
            _skillOwnerRepository.Update(new(skillOwner.Id, skillOwner.Skills, skillCooldowns));
        }

        public void Progress(SkillOwner skillOwner, float deltaTime)
        {
            Span<SkillCooldown> cooldowns = stackalloc SkillCooldown[skillOwner.Cooldowns.Length];
            skillOwner.Cooldowns.CopyTo(cooldowns);

            for (int i = 0; i < cooldowns.Length; i++)
            {
                SkillCooldown cooldown = cooldowns[i];

                if (cooldown.Value <= 0)
                {
                    continue;
                }

                cooldowns[i] = new(cooldown.Skill, cooldown.Value - deltaTime);
            }

            _skillOwnerRepository.Update(new(skillOwner.Id, skillOwner.Skills, cooldowns));
        }
    }
}
