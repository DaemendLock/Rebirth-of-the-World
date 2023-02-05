using System;
using UnityEngine;


public struct DamageEvent {
    public Unit victim;
    public Unit attacker;
    public float damage;
    public Ability ability;
    public DamageFlags damageFlags;
    public DamageEvent(Unit victim, Unit attacker, float damage, Ability ability = null, DamageFlags damageFlags = 0) {
        this.victim = victim;
        this.attacker = attacker;
        this.damage = damage;
        this.ability = ability;
        this.damageFlags = damageFlags;
    }
}

public class AttackEventInstance {
    public Unit attacker;
    public float damage;
    public DamageCategory damage_category;
    public Ability inflictor;
    public float original_damage;
    public Unit target;
    public DamageFlags damageFlags;
    public attackfail fail_type;

    public AttackEventInstance(Unit attacker, float damage, DamageCategory damage_category, Ability inflictor, float original_damage, Unit target, DamageFlags damageFlags) {
        this.attacker = attacker;
        this.damage = damage;
        this.damage_category = damage_category;
        this.inflictor = inflictor;
        this.original_damage = original_damage;
        this.target = target;
        this.damageFlags = damageFlags;
        fail_type = attackfail.NONE;
    }
}

public class UnitEventInstance {
    public Vector3 new_pos;
    public Unit unit;
    public UnitEventInstance(Vector3 new_pos, Unit unit) {
        this.unit = unit;
        this.new_pos = new_pos;
    }
}

public class AbilityEventInstance {
    private Ability _ability;
    public AbilityEventInstance(Ability ability) {
        _ability = ability;
    }

    public Ability Ability => _ability;
}

public static class EventManager {

    //Unit events
    public static event Action<AttackEventInstance> UnitAttacked;
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


    public static void SendAttackEvent(AttackEventInstance e) => UnitAttacked?.Invoke(e);

    public static void SendUnitEvent(UnitEventInstance e) => UnitEvent?.Invoke(e);

    public static void SendAbilityEvent(AbilityEventInstance e) => AbilityCasted?.Invoke(e);

    public static void SendDeathEvent(Unit e) => UnitDied?.Invoke(e);

    public static void SendTipCloseEvent() => TipClosed?.Invoke();

    public static void SendLocalGoalAchivedEvent() => GoalAchived?.Invoke();

    public static void SendLobbyTaskUpdateEvent(LobbyQuest quest, bool start) => LobbyQuestUpdated?.Invoke(quest, start);

    public static void SendScenarioFinishedEvent(Scenario scenario) => ScenarioComplited?.Invoke(scenario);

    public static void SendUnitResurectedEvent(Unit unit, float health) => UnitResurected?.Invoke(unit, health);
}
