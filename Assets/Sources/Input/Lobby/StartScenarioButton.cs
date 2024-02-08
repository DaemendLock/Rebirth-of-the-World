using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using View.General;

namespace Input.Lobby
{
    internal class StartScenarioButton : MonoBehaviour, IPointerClickHandler
    {
        public async void OnPointerClick(PointerEventData eventData)
        {//TODO
            //Networking.Combat.UseClient(new AutoAcceptClient());

            await Loader.LoadScene(1);
            await Task.Delay(1000);

            //UnitCreationData[] units = new UnitCreationData[eventData.Length + encounter.EncounterUnits.Length];

            //for (int i = 0; i < encounter.EncounterUnits.Length; i++)
            //{
            //    units[i] = encounter.EncounterUnits[i].GetUnitCreationData(i);
            //}

            //for (int i = 0; i < eventData.Length; i++)
            //{
            //    units[i + encounter.EncounterUnits.Length] = Character.Get(eventData[i].CharacterId).GetUnitCreationData(i + encounter.EncounterUnits.Length, 0, eventData[i]);
            //}

            //LoadChractersToCombat(units);
        }
    }
}
