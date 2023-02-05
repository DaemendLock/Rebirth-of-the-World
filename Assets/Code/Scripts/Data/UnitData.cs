using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    [CreateAssetMenu(fileName = "UnnamedUnit", menuName = "New Unit", order = 52)]
    public class UnitData : ScriptableObject {

        public UNIT_ROLE role;
        public ushort unitId;

        [Header("Base Stats")]
        public Stat Attack = new Stat(UnitStats.ATK);
        public Stat Spellpower = new Stat(UnitStats.SPELLPOWER);
        public Stat Crit = new Stat(UnitStats.CRIT);
        public Stat Versa = new Stat(UnitStats.VERSA);
        public Stat MaxHp = new Stat(UnitStats.MAXHP);
        public float[] maxResource = { 0, 0 };
        public float[] resourceRegen = { 0, 0 };
        public Stat Lifesteal = new Stat(UnitStats.LIFESTEAL);
        public Stat Movespeed = new Stat(UnitStats.MSPEED);
        public Stat AoeResist = new Stat(UnitStats.AOERESIST);
        public Stat Block = new Stat(UnitStats.BLOCK);
        public Stat Evade = new Stat(UnitStats.EVADE);
        public Stat Parry = new Stat(UnitStats.PARRY);
        public float RindRadius = 0.5f;

        [Header("Basic Attack")]
        public float AttackSpeed = 0;
        public float AttackRange = 0;
        public bool RangedAttack = false;
        public float ProjectileSpeed = 0;

        [Header("Abilities")]
        public List<AbilityData> abilities = new List<AbilityData>();

        [Header("Attached Graphics")]
        public Sprite GalleryCard;
        [SerializeField] private GameObject _unitPrefab;

        public GameObject Prefab => _unitPrefab;

        public StatsTable CalculateUnitStatsByRankAndLevel(byte rank, ushort level) {
            return StatsTable.FromDictionary(new Dictionary<UnitStats, float>() {
        { UnitStats.ATK, Attack.baseValue + level * Attack.gain[rank] },
        { UnitStats.SPELLPOWER, Spellpower.baseValue + level  * Spellpower.gain[rank] },
        { UnitStats.HASTE, 0 },
        { UnitStats.CRIT, Crit.baseValue + level * Crit.gain[rank] },
        { UnitStats.VERSA, Versa.baseValue + level * Versa.gain[rank] },
        { UnitStats.MAXHP, MaxHp.baseValue + level * MaxHp.gain[rank] },
        { UnitStats.MAXMANA, 0 },
        { UnitStats.LIFESTEAL, Lifesteal.baseValue + level  * Lifesteal.gain[rank] },
        { UnitStats.MSPEED, Movespeed.baseValue + level  * Movespeed.gain[rank]},
        { UnitStats.AOERESIST, AoeResist.baseValue + level * AoeResist.gain[rank] },
        { UnitStats.BLOCK, Block.baseValue + level * Block.gain[rank] },
        { UnitStats.EVADE, Evade.baseValue + level * Evade.gain[rank] },
        { UnitStats.PARRY, Parry.baseValue + level * Parry.gain[rank] }});
        }
        
        public void OnEnable() {
            unitId = RotW.StoreUnitData(this, unitId);
            RotW.prefabs[name] = _unitPrefab;

        }
    }

    [Serializable]
    public class Stat {
        [NonSerialized]
        public UnitStats type;
        public float baseValue;
        public float[] gain = { 0 };
        public Stat(UnitStats type) {
            this.type = type;
        }
    }
    public class Manatype {
        public static readonly Manatype MANA = new Manatype("mana", Color.blue);
        public static readonly Manatype RAGE = new Manatype("rage", Color.red);
        public static readonly Manatype ENERGY = new Manatype("energy", Color.yellow);

        string name;
        Color color;
        public Manatype(string name, Color color) {

        }
    }
}