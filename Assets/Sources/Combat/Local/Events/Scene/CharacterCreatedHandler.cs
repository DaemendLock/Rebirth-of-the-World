using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

using System;

namespace Combat.Local.Events
{
    public readonly ref struct CharacterCreatedInfo
    {
        public CharacterCreatedInfo(EntityId id, ReadOnlySpan<SkillId> initialSkills)
        {
            Id = id;
            InitialSkills = initialSkills;
        }

        public EntityId Id { get; }
        public ReadOnlySpan<SkillId> InitialSkills { get; }
    }

    public class CharacterCreatedHandler : ICreateUnitEventHandler
    {
        public delegate void Handler(CharacterCreatedInfo info);

        public event Handler Created;

        public void HandleEvent(EntityId id, ReadOnlySpan<SkillId> initalSkills)
        {
            CharacterCreatedInfo info = new(id, initalSkills);
            Created?.Invoke(info);
        }
    }
}
