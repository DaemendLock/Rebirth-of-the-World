using Combat.Common.ValueObjects;

using System;

using UnityEngine;

namespace Combat.Local.Controllers.Inputs
{
    public interface IHurtboxInput
    {
        event Action<HurtboxId> Destroied;
    }

    public class HurtboxInputComponent : MonoBehaviour, IHurtboxInput
    {
        public event Action<HurtboxId> Destroied;

        private void Awake()
        {
            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
            //UnityEngine.Debug.Log($"New hurtbox: {gameObject.name}");
        }

        public HurtboxId Id { get; set; }
    }
}
