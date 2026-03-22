using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

using UnityEngine;

namespace Combat.Local.Presentation.Presenters
{
    public class PlayerPresenter : ITakeControllOutput
    {
        private ICharacterViewContainer _container;

        public PlayerPresenter(ICharacterViewContainer container)
        {
            _container = container;
        }

        public void Present(EntityId id)
        {
            if (_container.TryGetValue(id, out var val) == false)
            {
                return;
            }

            Camera.main.transform.parent = val;
        }
    }
}
