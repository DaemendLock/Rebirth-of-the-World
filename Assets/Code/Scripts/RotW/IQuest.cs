using System;
using UnityEngine;

public interface IQuest {

    //Subscribe for event handling by quest
    public void StartQuest();

    //public void StopQuest();

    public void OnTrigger();
}



public class ReadTipQuest : IQuest {

    public string text;
    public ReadTipQuest(string tipText) {
        text = tipText;
    }
    public void OnTrigger() {
        EventManager.TipClosed -= OnTrigger;
        EventManager.SendLocalGoalAchivedEvent();
    }

    public void StartQuest() {
        TipPanel tipPanel = UI.Instance.tipPanel;
        tipPanel.textPanel.text = text;
        tipPanel.gameObject.SetActive(true);
        EventManager.TipClosed += OnTrigger;
    }
}

public class CastQuest : IQuest {

    //Ability to cast as quest body
    public ushort spellId;
    public Unit caster;

    public CastQuest(Ability ability) {
        spellId = ability.SpellId;
        caster = ability.Owner;
    }

    public CastQuest(ushort id) {
        spellId = id;
    }

    public void OnTrigger() {
        caster.AbilityUsed -= OnTrigger;
        caster.CastEnded -= OnTrigger;
        caster.ChannelEnded -= OnTrigger;
        EventManager.SendLocalGoalAchivedEvent();
    }

    public void OnTrigger(Ability ability, bool succes) {
        if (ability != null && ability.DataID == spellId && ability.Castable == true && succes)
            OnTrigger();
    }
    public void OnTrigger(Ability ability) {
        if (ability != null && ability.DataID == spellId && ability.Castable == false && ability.Channelable == false)
            OnTrigger();
    }

    public void OnTriggerChannel(Ability ability, bool succes) {
        if (ability != null && ability.DataID == spellId)
            OnTrigger();
    }

    private void CatchAbility(AbilityEventInstance eventInstance) {
        Debug.Log(eventInstance.Ability.SpellId == spellId);
        if (eventInstance.Ability.SpellId == spellId) {
            caster = eventInstance.Ability.Owner;
            OnTrigger(eventInstance.Ability);
            if (eventInstance.Ability.Castable || eventInstance.Ability.Channelable)
                StartQuest();
            EventManager.AbilityCasted -= CatchAbility;
        }
    }

    public void StartQuest() {
        if (caster == null) {
            EventManager.AbilityCasted += CatchAbility;
            return;
        }
        caster.AbilityUsed += OnTrigger;
        caster.CastEnded += OnTrigger;
        caster.ChannelEnded += OnTriggerChannel;
    }
}

public class HealQuest : IQuest {

    public Unit _target;

    public HealQuest(Unit target) {
        _target = target;
    }

    public void StartQuest() {
        _target.Healed += OnTrigger;
    }

    public void OnTrigger(HealthChangeEventInstance e) {
        if (_target.HealthPercent == 1)
            OnTrigger();
    }

    public void OnTrigger() {
        _target.Healed -= OnTrigger;
        EventManager.SendLocalGoalAchivedEvent();
    }

}

[Serializable]
public class LobbyQuest : IQuest {

    public string title;
    public string description;
    public int id;
    public string scenarioName;

    public bool IsComplite { get; private set; } = false;

    public void StartQuest() {
        if (scenarioName != null) {
            EventManager.ScenarioComplited += OnTrigger;
        }
        EventManager.SendLobbyTaskUpdateEvent(this, true);
    }

    public void OnTrigger(Scenario scenario) {
        if (scenario.ScenarioName == scenarioName) {
            OnTrigger();
        }
    }

    public static LobbyQuest GetQuestById(int id) {
        TextAsset json = Resources.Load("Quests/" + id) as TextAsset;
        return JsonUtility.FromJson<LobbyQuest>(json.text);
    }

    public void OnTrigger() {
        IsComplite = true;
        EventManager.SendLobbyTaskUpdateEvent(this, false);
    }
}