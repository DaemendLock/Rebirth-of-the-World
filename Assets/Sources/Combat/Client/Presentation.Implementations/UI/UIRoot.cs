using Client.Combat.Presentation.UI.Panels;

using UnityEngine;

namespace Client.Combat.Presentation.UI
{
    public class UIRoot : MonoBehaviour
    {
        public static UIRoot Instance { get; private set; }

        [field: SerializeField] public ActionBar.ActionBar ActionBar { get; private set; }
        [field: SerializeField] public ResourceBar.ResourceBar ResourceBar { get; private set; }
        [field: SerializeField] public UnitPanel UnitPanel { get; private set; }
        [field: SerializeField] public UnitPanel TargetPanel { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                gameObject.SetActive(false);
                throw new System.InvalidOperationException($"Instance of {typeof(UIRoot)} already exists.");
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
