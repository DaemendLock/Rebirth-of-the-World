using TMPro;
using UnityEngine;

namespace Client.Lobby.View.MainMenu
{
    public class Taskboard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _questTitle;
        [SerializeField] private TMP_Text _questDesription;

        //private readonly List<LobbyQuest> lobbyQuest = new();
        //TODO... one day

        private void Start()
        {
            //EventManager.LobbyQuestUpdated += UpdateQuest;
        }

        /*
        public void UpdateQuest(LobbyQuest quest, bool start)
        {
            if (start)
            {
                AddQuest(quest);
            }
            if (quest.IsComplite)
            {
                FinishQuest(quest);
            }
        }

        private void AddQuest(LobbyQuest quest)
        {
            lobbyQuest.Add(quest);
            if (lobbyQuest.Count == 1)
            {
                _questTitle.text = quest.title;
                _questDesription.text = quest.description;
            }
        }

        private void FinishQuest(LobbyQuest quest)
        {
            lobbyQuest.Remove(quest);
            if (lobbyQuest.Count == 0)
            {
                _questTitle.text = "No Active tasks";
                _questDesription.text = "";
                return;
            }
            _questTitle.text = lobbyQuest[0].title;
            _questDesription.text = lobbyQuest[0].description;
        }

        private void OnDestroy()
        {
            EventManager.LobbyQuestUpdated -= UpdateQuest;
        }
        */
    }
}