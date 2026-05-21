using Lobby.Local.Domain.UseCases.Scenarios;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Lobby.Local.Presentation.View.ScenarioSelection
{
    public sealed class JoinScenarioButton : MonoBehaviour, IPointerClickHandler
    {
        [Zenject.Inject] private readonly ScenarioJoinUseCase _scenarioJoinUseCase;

        public void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}
