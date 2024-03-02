using Client.Combat.View.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Combat.View.UI.Nameplates
{
    public class NameplatesRoot : MonoBehaviour
    {
        private static Queue<Unit> _creationQueue = new Queue<Unit>();

        [SerializeField] private Camera _camera;
        [SerializeField] private Nameplate _nameplatePrefab;

        private List<Nameplate> _nameplates = new List<Nameplate>();

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

        private void LateUpdate()
        {
            foreach (Nameplate nameplate in _nameplates)
            {
                if (nameplate.enabled)
                {
                    nameplate.UpdatePosition(_camera);
                }
            }
        }

        public void ShowSelelction(int unitId)
        {
            foreach (Nameplate nameplate in _nameplates)
            {
                nameplate.InformSelectionChanged(unitId);
            }
        }
        public void ShowTarget(int unitId)
        {
            foreach (Nameplate nameplate in _nameplates)
            {
                nameplate.SetTargeted(unitId == nameplate.AssignedUnit.Id);
            }
        }

        public static Nameplate CreateNameplate(Unit unit)
        {
            if (Instance == null)
            {
                _creationQueue.Enqueue(unit);
                return null;
            }

            Nameplate result = Instantiate(Instance._nameplatePrefab, Instance.transform);
            Instance._nameplates.Add(result);
            result.AssignTo(unit);
            return result;
        }
    }
}
