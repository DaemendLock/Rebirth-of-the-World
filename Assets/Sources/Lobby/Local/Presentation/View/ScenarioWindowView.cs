using Lobby.Local.Presentation.ViewModels.ScenarioSelection;
using Lobby.Local.Presentation.Widgets;

using System.Collections.Generic;

using UnityEngine;

namespace Lobby.Local.Presentation.View
{
    public sealed class ScenarioWindowView : MonoBehaviour
    {
        private readonly Stack<ScenarioCardWidget> _cardPool = new();
        private readonly List<ScenarioCardWidget> _activeCards;

        [SerializeField] private ScenarioCardWidget _prefab;
        [SerializeField] private Transform _rootContainer;

        public void Show(IReadOnlyCollection<ScenarioViewModel> values)
        {
            _activeCards.ForEach(value =>
            {
                value.gameObject.SetActive(false);
                _cardPool.Push(value);
            });

            _activeCards.Clear();

            foreach (ScenarioViewModel viewModel in values)
            {
                ScenarioCardWidget card = GetCard();
                card.Icon = viewModel.Icon;
                card.LocalizedName = viewModel.LocalizedName;
                card.gameObject.SetActive(true);
                _activeCards.Add(card);
            }
        }

        private ScenarioCardWidget GetCard()
        {
            if (_cardPool.TryPop(out var result))
            {
                return result;
            }

            result = Instantiate(_prefab, _rootContainer);
            return result;
        }
    }
}