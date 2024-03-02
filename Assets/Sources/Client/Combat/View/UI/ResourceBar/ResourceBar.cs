using Client.Combat.View.UI.Elements;
using UnityEngine;
using Utils.DataTypes;

namespace Client.Combat.View.UI.ResourceBar
{
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] private Bar _leftBar;
        [SerializeField] private Bar _rightBar;

        public void UpdateValues(Resource left, Resource right)
        {
            _leftBar.MaxValue = left.MaxValue;
            _leftBar.Value = left.Value;

            _rightBar.MaxValue = right.MaxValue;
            _rightBar.Value = right.Value;
        }

        public void UpdateResourceTypes(ResourceType left, ResourceType right)
        {
            _leftBar.SetResourceType(left);
            _rightBar.SetResourceType(right);
        }
    }
}
