using Game.Domain.ValueObjects;

using Lobby.Common.Primitives;

using System;

namespace Game.Domain.Entities
{
    public sealed class Scenario
    {
        private readonly ScenarioMemberInfo?[] _members;

        public Scenario(ScenarioId id, string locationName, ScenarioMemberInfo?[] members)
        {
            Id = id;
            LocationName = locationName;
            _members = members;
        }

        public ScenarioId Id { get; }
        public string Name { get; set; }
        public string LocationName { get; set; }

        public void Join(AccountId accountId)
        {
            for (int i = 0; i < _members.Length; i++)
            {
                if (_members[i].HasValue)
                {
                    continue;
                }

                _members[i] = new(accountId, default, default);
                return;
            }

            throw new InvalidOperationException($"Player(Id: {accountId}) can't join scenario(Id:{Id}): Scenario is full");
        }

        public void Leave(AccountId accountId)
        {
            for (int i = 0; i < _members.Length; i++)
            {
                ScenarioMemberInfo? member = _members[i];

                if (member.HasValue == false)
                {
                    continue;
                }

                if (member.Value.AccountId != accountId)
                {
                    continue;
                }

                _members[i] = null;
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

            for (int i = 0; i < _members.Length; i++)
            {
                var item = _members[i];

                if (item.HasValue == false)
                {
                    continue;
                }

                if (item.Value.AccountId != accountId)
                {
                    continue;
                }

                _members[i] = new(accountId, characterSelection?.CharacterKey, item.Value.TeamId);
                return;
            }
        }
        private bool IsAllowed(CharacterSelection? character)
        {
            if (character.HasValue == false)
            {
                return true;
            }

            foreach (var selection in _members)
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
