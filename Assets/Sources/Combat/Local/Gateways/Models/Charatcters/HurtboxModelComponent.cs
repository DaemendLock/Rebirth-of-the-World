using Combat.Common.Primitives;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    public class HurtboxModelComponent : MonoBehaviour
    {
        private void Awake()
        {
            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
        }

        public HurtboxType Type { get; set; }

        public UnitId Owner { get; set; }
    }
}
