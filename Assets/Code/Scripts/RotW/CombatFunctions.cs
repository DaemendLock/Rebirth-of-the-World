using Data;
using System;
using System.Collections.Generic;
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
    public static Dictionary<ushort, UnitData> unitDataList = new Dictionary<ushort, UnitData>();

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

    public static Unit CreateUnitByName(string unitName, Vector3 location, Team team, bool controlable = false, byte rank = 0, ushort lvl = 1) {
        return CreateUnitByName(unitName, new SpawnLocation(location, Quaternion.Euler(Vector3.zero)), team, controlable, rank, lvl);
    }

    public static Unit CreateUnitByName(string unitName, SpawnLocation location, Team team, bool controlable = false, byte rank = 0, ushort lvl = 1) {
        GameObject prefab;
        if (!Prefabs.TryGetValue(unitName, out prefab)) {
            Debug.LogError("No such UnitType: " + unitName);
            return null;
        }
        return CreateUnitByName(prefab, location, team, controlable, rank, lvl);
    }

    public static Unit CreateUnitByName(GameObject prefab, SpawnLocation location, Team team, bool controlable = false, byte rank = 0, ushort lvl = 1) {
        GameObject newUnit = UnityEngine.Object.Instantiate(prefab, location.Position, location.Direction);
        newUnit.name = prefab.name;
        Unit res = newUnit.GetComponent<Unit>();

        res.SetTeam(team);

        if (team == Team.TEAM_ALLY && controlable) {
            Controller.Instance.AddSelectableUnit(res);
            Controller.Instance.aliveAlly++;
        }
        return res;
    }

    public static List<Unit> FindUnitsInRadius(Vector3 location, float radius, Unit cacheUnit = null, UNIT_TARGET_TEAM filter = UNIT_TARGET_TEAM.BOTH, bool includSelf = true) {
        List<Unit> res = new List<Unit>();
        Collider[] hits = Physics.OverlapSphere(location, radius);
        Unit cur;
        foreach (Collider hit in hits) {
            cur = hit.gameObject.GetComponent<Unit>();
            if (cur != null && (filter == UNIT_TARGET_TEAM.BOTH || (cacheUnit != null &&
                (cacheUnit.Team == cur.Team && filter == UNIT_TARGET_TEAM.FRIENDLY && (includSelf || (cacheUnit != cur)) || cacheUnit.Team != cur.Team && filter == UNIT_TARGET_TEAM.NONE)))) {
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

    public static UnitData GetUnitDataById(ushort id) {
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
        if (type == ResourceType.SPRITE)
            Sprites[name] = Resources.Load<Sprite>(resource);
        else if (type == ResourceType.PREFAB)
            Prefabs[name] = Resources.Load<GameObject>(resource);
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

    public static ushort StoreUnitData(UnitData data, ushort preferId = 0) {
        unitDataList ??= new Dictionary<ushort, UnitData>();
        if (unitDataList.ContainsKey(preferId)) {
            throw new Exception("Duplicate unit id: " + preferId);
        }
        unitDataList[preferId] = data;
        //abilitiesId[data.abilityName] = preferId;
        return preferId;
    }

}

