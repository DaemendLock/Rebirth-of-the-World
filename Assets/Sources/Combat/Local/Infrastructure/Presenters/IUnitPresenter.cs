using Combat.Local.Presentation.Components;

using UnityEngine;

namespace Combat.Local.Infrastructure.Presenters
{
    public interface IUnitPresenter
    {
        Vector3 Position { get; set; }

        Vector3 Velocity { get; set; }

        void SetModel(CharacterView characterView);

        void PlayAnimation(AnimationClip clip);
    }
}
