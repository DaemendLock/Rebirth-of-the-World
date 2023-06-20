using Combat.SpellOld;
using Combat.Status;
using Data;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum ResourceType {
    SPRITE,
    SOUND,
    PREFAB,
    TOKEN
}

public struct LateCache {
    public string name;
    public string path;
    public ResourceType type;
    public LateCache(string name, string path, ResourceType type) {
        this.name = name;
        this.path = path;
        this.type = type;
    }
}

public static class RotW {
    //Constants
    public static float DAMAGE_BYPASS_BLOCK = 0.5f;

    //Fields
    private static List<LateCache> toLateCache = new List<LateCache>();
    public static bool earlyPrecache = true;
    //Abilities
    private static Dictionary<ushort, AbilityData> _abilityDataList;
    private static Dictionary<string, ushort> _abilitiesId = new Dictionary<string, ushort>();

    public static Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
    public static Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();

    //Units
    public static Dictionary<ushort, OldUnitData> unitDataList = new Dictionary<ushort, OldUnitData>();

    public static void ApplyDamage(DamageEvent damageEvent) {
        AttackEventInstance e = new AttackEventInstance(damageEvent.Attacker, damageEvent.Victim, damageEvent.Ability, damageEvent.Damage, damageEvent.Damage, damageEvent.Ability == null ? DamageCategory.ATTACK : DamageCategory.SPELL, damageEvent.DamageFlags);
        damageEvent.Victim.Damage(e);
        if (e.FailType != attackfail.MISS && e.FailType == attackfail.DEAD)
            EventManager.SendAttackEvent(e);
    }

    public static void ApplyHealing(HealEvent healEvent) {
        HealEventInstance e = new(healEvent.Healer, healEvent.Target, healEvent.Ability, healEvent.Healing, healEvent.Healing, healEvent.HealingFlags);
        healEvent.Target.Heal(e);
        if (e.FailType != attackfail.DEAD) {
            EventManager.SendHealEvent(e);
        }
    }

    public static void LinkStatus(string name, Type type) {
        Status.allStatusList.Add(name, type);
    }

    public static bool CheckDistance(Unit unit1, Unit unit2, float distnce) {
        return unit1.RingRadius + distnce + unit2.RingRadius >= (unit1.Origin - unit2.Origin).magnitude;
    }

    public static Unit CreateUnitByType(UnitFactory.UnitType type, SpawnLocation location, Team team, int serverId, bool controlable = false, byte rank = 0, ushort lvl = 1) {
        Unit res = new Florence(location.Position, location.Direction, team, (byte) lvl, rank, 0);
        
        if (team == Team.ALLY && controlable) {
            Controller.Instance.AddSelectableUnit(res);
            Controller.Instance.aliveAlly++;
        }
        return res;
    }

    public static List<Unit> FindUnitsInRadius(Vector3 location, float radius, Unit cacheUnit = null, UnitTargetTeam filter = UnitTargetTeam.BOTH, bool includSelf = true) {
        List<Unit> res = new List<Unit>();
        Collider[] hits = Physics.OverlapSphere(location, radius);
        Unit cur;
        foreach (Collider hit in hits) {
            cur = hit.gameObject.GetComponent<Unit>();
            if (cur != null && (filter == UnitTargetTeam.BOTH || (cacheUnit != null &&
                (cacheUnit.Team == cur.Team && filter == UnitTargetTeam.FRIENDLY && (includSelf || (cacheUnit != cur)) || cacheUnit.Team != cur.Team && filter == UnitTargetTeam.NONE)))) {
                res.Add(cur);
            }
        }
        return res;
    }

    public static AbilityData GetAbilityDataById(ushort id) {
        if (!_abilityDataList.ContainsKey(id)) {
            Debug.LogError("Failed to get ability by id: " + id);
            return null;
        }
        return _abilityDataList[id];
    }

    public static ushort GetAbilityIdByName(string name) {
        return _abilitiesId[name];
    }

    public static OldUnitData GetUnitDataById(ushort id) {
        if (!unitDataList.ContainsKey(id)) {
            Debug.LogError("Failed to get unit by id: " + id);
            return null;
        }
        return unitDataList[id];
    }

    public static void Precache(Type precache) {
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(precache.TypeHandle);
    }

    public static void Precache(string name, string resource, ResourceType type) {
        switch (type) {
            case ResourceType.SPRITE:
                Sprites[name] = Resources.Load<Sprite>(resource);
                break;
            case ResourceType.PREFAB:
                Prefabs[name] = Resources.Load<GameObject>(resource);
                break;
        }
    }

    public static void PrecacheLate() {
        earlyPrecache = false;
        LateCache c;
        while (toLateCache.Count > 0) {
            c = toLateCache[0];
            Precache(c.name, c.path, c.type);
            toLateCache.RemoveAt(0);
        }

        toLateCache.Clear();
    }

    public static bool RollPercentage(float probability) {
        return probability > UnityEngine.Random.Range(0f, 100.0f);
    }

    public static float RandomFloat(float min, float max) {
        return 0;
    }

    public static int RandomInt(int min, int max) {
        return 0;
    }

    public static ushort StoreAbilityData(AbilityData data, ushort preferId = 0) {
        _abilityDataList ??= new Dictionary<ushort, AbilityData>();
        if (_abilityDataList.ContainsKey(preferId)) {
            Debug.LogError("Duplicate ability data id: " + preferId);
            return preferId;
        }
        _abilityDataList[preferId] = data;
        _abilitiesId[data.GetName()] = preferId;
        return preferId;
    }

    public static ushort StoreUnitData(OldUnitData data, ushort preferId = 0) {
        unitDataList ??= new Dictionary<ushort, OldUnitData>();
        if (unitDataList.ContainsKey(preferId)) {
            throw new Exception("Duplicate unit id: " + preferId);
        }
        unitDataList[preferId] = data;
        //abilitiesId[data.abilityName] = preferId;
        return preferId;
    }

    public static class UnitFactory {
        public enum UnitType : ushort {
            FLORENCE = Florence.UNIT_ID,
            GLORIA = Gloria.UNIT_ID,
        }

        private static readonly Dictionary<UnitType, Type> unitClasses = new() {
            [UnitType.FLORENCE] = typeof(Florence),
            [UnitType.GLORIA] = typeof(Gloria)
        };

        public static Unit CreateUnit(UnitType type, Vector3 location, Quaternion facing, Team team, ushort lvl, ushort rank, int objectId) {
            return (Unit) Activator.CreateInstance(unitClasses[type], location, facing, team, lvl, rank, objectId);
        }
    }
}

