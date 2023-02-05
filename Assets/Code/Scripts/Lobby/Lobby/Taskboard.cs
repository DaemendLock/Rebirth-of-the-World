using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Taskboard : MonoBehaviour {

    [SerializeField] private TMP_Text questTitle;
    [SerializeField] private TMP_Text questDesription;

    private readonly List<LobbyQuest> lobbyQuest = new();

    private void Start() {
        EventManager.LobbyQuestUpdated += UpdateQuest;
    }

    public void UpdateQuest(LobbyQuest quest, bool start) {
        if (start) {
            AddQuest(quest);
        }
        if (quest.IsComplite) {
            FinishQuest(quest);
        }
    }

    private void AddQuest(LobbyQuest quest) {
        lobbyQuest.Add(quest);
        if (lobbyQuest.Count == 1) {
            questTitle.text = quest.title;
            questDesription.text = quest.description;
        }
    }

    private void FinishQuest(LobbyQuest quest) {
        lobbyQuest.Remove(quest);
        if (lobbyQuest.Count == 0) {
            questTitle.text = "No Active tasks";
            questDesription.text = "";
            return;
        }
        questTitle.text = lobbyQuest[0].title;
        questDesription.text = lobbyQuest[0].description;
    }

    private void OnDestroy() {
        EventManager.LobbyQuestUpdated -= UpdateQuest;
    }

}
