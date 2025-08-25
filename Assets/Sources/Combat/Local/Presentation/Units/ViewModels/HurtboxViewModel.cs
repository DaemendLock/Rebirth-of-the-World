using System.Collections.Generic;

using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Presentation.Units.ViewModels
{
    public class HurtboxViewModel : MonoBehaviour
    {
        private void Awake()
        {
            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
            //UnityEngine.Debug.Log($"New hurtbox: {gameObject.name}");
        }

        public HurtboxId Id { get; set; }
        public HurtboxType Type { get; set; }

        public override bool Equals(object obj) => obj is HurtboxViewModel model && base.Equals(obj) && EqualityComparer<HurtboxId>.Default.Equals(Id, model.Id);
        public override int GetHashCode() => Id.Value;
    }
}
