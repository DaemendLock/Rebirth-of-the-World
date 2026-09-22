using Lobby.Local.Application.UseCases.Scenarios;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Lobby.Local.Presentation.View.ScenarioSelection
{
    public sealed class JoinScenarioButton : MonoBehaviour, IPointerClickHandler
    {
        [Zenject.Inject] private readonly ScenarioRequestJoinUseCase _scenarioJoinUseCase;

        public void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}
