using Client.Combat.Presentation.Implementations.Units;
using Client.Combat.Presentation.Units;

using System.Collections.Generic;

using UnityEngine;

namespace Client.Combat.Presentation.UI.Nameplates
{
    public class NameplatesRoot : MonoBehaviour
    {
        private static Queue<UnitPresenter> _creationQueue = new();

        [SerializeField] private Camera _camera;
        [SerializeField] private Nameplate _nameplatePrefab;

        private List<Nameplate> _nameplates = new();

        public static NameplatesRoot Instance { get; private set; }

        private void Start()
        {
            if (Instance != null)
            {
                gameObject.SetActive(false);
                return;
            }

            Instance = this;

            while (_creationQueue.Count > 0)
            {
                CreateNameplate(_creationQueue.Dequeue());
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
                _creationQueue.Clear();
            }
        }

        public static Nameplate CreateNameplate(UnitPresenter unit)
        {
            if (Instance == null)
            {
                _creationQueue.Enqueue(unit);
                return null;
            }

            Nameplate result = Instantiate(Instance._nameplatePrefab, Instance.transform);
            Instance._nameplates.Add(result);
            return result;
        }
    }
}
