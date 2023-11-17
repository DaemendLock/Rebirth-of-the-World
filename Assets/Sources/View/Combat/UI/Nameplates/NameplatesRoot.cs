using System.Collections.Generic;
using UnityEngine;
using View.Combat.Units;

namespace View.Combat.UI.Nameplates
{
    public class NameplatesRoot : MonoBehaviour
    {
        private static NameplatesRoot _instance;

        [SerializeField] private UnityEngine.Camera _camera;
        public static Nameplate NameplatePrefab;

        private List<Nameplate> _nameplates = new List<Nameplate>();

        private void Start()
        {
            if (_instance != null)
            {
                gameObject.SetActive(false);
                return;
            }

            _instance = this;
        }

        private void LateUpdate()
        {
            foreach (Nameplate nameplate in _nameplates)
            {
                if (nameplate.enabled)
                    nameplate.UpdatePostiotn(_camera);
            }
        }

        public static Nameplate CreateNameplate(int id)
        {
            Nameplate result = Instantiate(NameplatePrefab, _instance.transform);
            _instance._nameplates.Add(result);
            result.AssignTo(id);
            return result;
        }
    }
}
