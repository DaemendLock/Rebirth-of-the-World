using Core.Lobby.Encounters;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View.Lobby.ScenarionSelection.Widgets
{
    internal class ScenarioOptionWidget : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _scenarioName;
        [SerializeField] private Encounter _encounter;

        //private ScenarioData _data;
        //

        private void Start()
        {
            _scenarioName.text = _encounter.Name;
        }

        public void Init(/*ScenarioData data*/)
        {
            // _image = Get image for scenario
            // _scenarioName = Get localized name of scenario
            // _data = data;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Send server request to open team setup window
            Lobby.Instance?.StartScenario(_encounter);
        }
    }
}
