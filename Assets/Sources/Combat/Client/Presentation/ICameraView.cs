using UnityEngine;

using UtilsUnity.Patterns.View;

namespace Client.Combat.Presentation
{
    public interface ICameraView : IBindableView<Transform>
    {
        Vector2 Rotation { get; set; }
    }
}
