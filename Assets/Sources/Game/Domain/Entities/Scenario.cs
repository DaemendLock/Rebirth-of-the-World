using Game.Domain.ValueObjects;

using Lobby.Common.Primitives;

using System;

namespace Game.Domain.Entities
{
    public sealed class Scenario
    {
        public ScenarioMemberInfo?[] Members { get; }

        public Scenario(ScenarioId id, string locationName, ScenarioMemberInfo?[] members)
        {
            Id = id;
            LocationName = locationName;
            Members = members;
        }

        public ScenarioId Id { get; }
        public string Name { get; set; }
        public string LocationName { get; set; }

        public void Join(AccountId accountId)
        {
            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i].HasValue)
                {
                    continue;
                }

                Members[i] = new(accountId, default, default);
                return;
            }

            throw new InvalidOperationException($"Player(Id: {accountId}) can't join scenario(Id:{Id}): Scenario is full");
        }

        public void Leave(AccountId accountId)
        {
            for (int i = 0; i < Members.Length; i++)
            {
                ScenarioMemberInfo? member = Members[i];

                if (member.HasValue == false)
                {
                    continue;
                }

                if (member.Value.AccountId != accountId)
                {
                    continue;
                }

                Members[i] = null;
                return;
            }

            throw new InvalidOperationException($"Player(Id: {accountId}) can't leave scenario(Id:{Id}): Not in scenario");
        }

        public void SelecetCharacter(AccountId accountId, CharacterSelection? characterSelection)
        {
            if (IsAllowed(characterSelection) == false)
            {
                throw new InvalidOperationException($"Player(Id: {accountId}) can't select character: Not allowed");
            }

            for (int i = 0; i < Members.Length; i++)
            {
                var item = Members[i];

                if (item.HasValue == false)
                {
                    continue;
                }

                if (item.Value.AccountId != accountId)
                {
                    continue;
                }

                Members[i] = new(accountId, characterSelection?.CharacterKey, item.Value.TeamId);
                return;
            }
        }
        private bool IsAllowed(CharacterSelection? character)
        {
            if (character.HasValue == false)
            {
                return true;
            }

            foreach (var selection in Members)
            {
                if (selection.HasValue == false)
                {
                    continue;
                }

                if (selection.Value.Character == character?.CharacterKey)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
