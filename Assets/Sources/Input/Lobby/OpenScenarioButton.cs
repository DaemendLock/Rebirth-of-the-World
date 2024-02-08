using Core.Lobby.Encounters;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Lobby.ScenarionSelection.Widgets;

namespace Input.Lobby
{
    [RequireComponent(typeof(ScenarioOptionWidget))]
    internal class OpenScenarioButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Encounter _encounter;

        private ScenarioOptionWidget _view;

        private void Start()
        {
            _view = GetComponent<ScenarioOptionWidget>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //TODO:: Networking.Lobby.SendStartScenario

            View.Lobby.Lobby.Instance?.StartScenario(_encounter);
        }
    }
}
