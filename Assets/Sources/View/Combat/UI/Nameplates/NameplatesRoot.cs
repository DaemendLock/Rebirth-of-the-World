using System.Collections.Generic;
using UnityEngine;
using View.Combat.Units;

namespace View.Combat.UI.Nameplates
{
    public class NameplatesRoot : MonoBehaviour
    {
        private static Queue<int> _creationQueue = new Queue<int>();

        [SerializeField] private UnityEngine.Camera _camera;
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
                    nameplate.UpdatePostiotn(_camera);
                }
            }
        }

        public void ShowSelelction(int unitId)
        {
            foreach (Nameplate nameplate in _nameplates)
            {
                nameplate.SetSellected(unitId == nameplate.AssignedId);
            }
        }
        public void ShowTarget(int unitId)
        {
            foreach (Nameplate nameplate in _nameplates)
            {
                //nameplate.SetSellected(unitId == nameplate.AssignedId);
            }
        }

        public static Nameplate CreateNameplate(int id)
        {
            if (Instance == null)
            {
                _creationQueue.Enqueue(id);
                return null;
            }

            Nameplate result = Instantiate(Instance._nameplatePrefab, Instance.transform);
            Instance._nameplates.Add(result);
            result.AssignTo(id);
            return result;
        }
    }
}
