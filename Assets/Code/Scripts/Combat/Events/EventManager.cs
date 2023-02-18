using System;


public static class EventManager {

    //Unit events
    public static event Action<AttackEventInstance> UnitAttacked;
    public static event Action<HealEventInstance> UnitHealed;
    public static event Action<HealthChangeEventInstance> UnitHealthChanged;
    public static event Action<Unit> UnitDied;
    public static event Action<Unit, float> UnitResurected;
    public static event Action<UnitEventInstance> UnitEvent;
    public static event Action<AbilityEventInstance> AbilityCasted;

    //UI events
    public static event Action TipClosed;
    public static event Action GoalAchived;
    public static event Action<LobbyQuest, bool> LobbyQuestUpdated;
    public static event Action<Scenario> ScenarioComplited;
    public static event Action<Scenario> ScenarioAbortred;


    public static void SendAttackEvent(AttackEventInstance e) { UnitAttacked?.Invoke(e); SendHealthChangeEvent(e); }

    public static void SendHealEvent(HealEventInstance e) { UnitHealed?.Invoke(e); SendHealthChangeEvent(e); }

    public static void SendHealthChangeEvent(HealthChangeEventInstance e) => UnitHealthChanged?.Invoke(e);

    public static void SendUnitEvent(UnitEventInstance e) => UnitEvent?.Invoke(e);

    public static void SendAbilityEvent(AbilityEventInstance e) => AbilityCasted?.Invoke(e);

    public static void SendDeathEvent(Unit e) => UnitDied?.Invoke(e);

    public static void SendTipCloseEvent() => TipClosed?.Invoke();

    public static void SendLocalGoalAchivedEvent() => GoalAchived?.Invoke();

    public static void SendLobbyTaskUpdateEvent(LobbyQuest quest, bool start) => LobbyQuestUpdated?.Invoke(quest, start);

    public static void SendScenarioFinishedEvent(Scenario scenario) => ScenarioComplited?.Invoke(scenario);

    public static void SendUnitResurectedEvent(Unit unit, float health) => UnitResurected?.Invoke(unit, health);
}
