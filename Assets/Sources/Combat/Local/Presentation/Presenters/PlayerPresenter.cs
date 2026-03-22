using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;
using Combat.Local.Presentation.Components;

namespace Combat.Local.Presentation.Presenters
{
    public class PlayerPresenter : ITakeControllOutput
    {
        private ICharacterViewContainer _container;
        
        private CameraView _cameraView;

        public PlayerPresenter(ICharacterViewContainer container, CameraView cameraView)
        {
            _container = container;
            _cameraView = cameraView;
        }

        public void Present(EntityId id)
        {
            if (_container.TryGetValue(id, out var val) == false)
            {
                return;
            }

            _cameraView.Follow(val);
        }
    }
}
