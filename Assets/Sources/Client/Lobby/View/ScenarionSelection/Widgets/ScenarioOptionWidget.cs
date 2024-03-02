using Client.Lobby.Core.Encounter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.Lobby.ScenarionSelection.Widgets
{
    public class ScenarioOptionWidget : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _scenarioName;

        public void SetScenario(Encounter encounter)
        {
            //_image = Get image for scenario
            _scenarioName.text = encounter.Name;
            // _data = data;
        }
    }
}
