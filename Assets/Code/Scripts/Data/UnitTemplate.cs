using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    [CreateAssetMenu(fileName = "UnnamedUnit", menuName = "New Unit", order = 52)]
    public class UnitTemplate : ScriptableObject {
        public UNIT_ROLE role;
        public ushort unitId;
        public string Name;
        [Header("Base Stats")]
        public Stat Attack = new();
        public Stat Spellpower = new();
        public Stat Crit = new();
        public Stat Haste = new();
        public Stat Versa = new();
        public Stat MaxHp = new();
        public float[] maxResource = { 0, 0 };
        public float[] resourceRegen = { 0, 0 };
        public Stat Lifesteal = new();
        public Stat Movespeed = new();
        public Stat AoeResist = new();
        public Stat Block = new();
        public Stat Evade = new();
        public Stat Parry = new();
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

        private void OnEnable() {
            new UnitData(this);
        }
    }
    public class UnitData {
        public UNIT_ROLE Role = UNIT_ROLE.NONE;
        public string Name = "Empity Name";
        public Stat Attack = new() { baseValue = 0, gain = new float[] { 0 } };
        public Stat Spellpower = new() { baseValue = 0, gain = new float[] { 0 } };
        public Stat Crit = new() { baseValue = 0, gain = new float[] { 0 } };
        public Stat Haste = new() { baseValue = 0, gain = new float[] { 0 } };
        public Stat Versa = new() { baseValue = 0, gain = new float[] { 0 } };
        public Stat MaxHp = new() { baseValue = 0, gain = new float[] { 0 } };
        public float[] MaxResource;
        public float[] ResourceRegen;
        public Stat Lifesteal = new() { baseValue = 0, gain = new float[] { 0 } };
        public Stat Movespeed = new() { baseValue = 0, gain = new float[] { 0 } };
        public Stat AoeResist = new() { baseValue = 0, gain = new float[] { 0 } };
        public Stat Block = new() { baseValue = 0, gain = new float[] { 0 } };
        public Stat Evade = new() { baseValue = 0, gain = new float[] { 0 } };
        public Stat Parry = new() { baseValue = 0, gain = new float[] { 0 } };
        public float RindRadius = 0.5f;
        public float AttackSpeed = 1f;
        public float AttackRange = 0f;
        public bool RangedAttack = false;
        public float ProjectileSpeed = 0f;
        public List<AbilityData> Abilities = new();
        private readonly ushort _unitId;
        public ushort UnitId => _unitId;
        //Viusal
        public string GalleryCardName;
        public string UnitPrefabName;
        private Sprite _galleryCard;
        private GameObject _unitPrefab;

        public Sprite GalleryCard => _galleryCard ??= _galleryCard = RotW.Sprites[GalleryCardName];
        public GameObject UnitPrefab { 
            get { 
                _unitPrefab ??= _unitPrefab = RotW.Prefabs[UnitPrefabName]; 
                return _unitPrefab;
            } 
        }




        public UnitData(ushort unitId) { _unitId = unitId; }

        public UnitData(UnitTemplate data) {
            Role = data.role;
            Name = data.name;
            Attack = data.Attack;
            Spellpower = data.Spellpower;
            Haste = data.Haste;
            Crit = data.Crit;
            Versa = data.Versa;
            MaxHp = data.MaxHp;
            MaxResource = data.maxResource;
            ResourceRegen = data.resourceRegen;
            Lifesteal = data.Lifesteal;
            Movespeed = data.Movespeed;
            AoeResist = data.AoeResist;
            Block = data.Block;
            Evade = data.Evade;
            Parry = data.Parry;
            RindRadius = data.RindRadius;
            AttackSpeed = data.AttackSpeed;
            AttackRange = data.AttackRange;
            RangedAttack = data.RangedAttack;
            ProjectileSpeed = data.ProjectileSpeed;
            Abilities = data.abilities;
            _galleryCard = data.GalleryCard;
            _unitPrefab = data.Prefab;
            Debug.Log("Unit prefab = " + UnitPrefab);
            RotW.StoreUnitData(this, data.unitId);
            _unitId = data.unitId;
            RotW.Prefabs[data.name] = UnitPrefab;
        }

        public StatsTable CalculateUnitStatsByRankAndLevel(byte rank, ushort level) => new () {
            Atk = Attack.baseValue + level * Attack.gain[rank],
            AtkPercent = 100,
            Spellpower = Spellpower.baseValue + level * Spellpower.gain[rank],
            SpellpowerPercent = 100,
            Crit = Crit.baseValue + level * Crit.gain[rank],
            CritPercent = 100,
            Haste = Haste.baseValue + level * Haste.gain[rank],
            HastePercent = 100,
            Versality = Versa.baseValue + level * Versa.gain[rank],
            VersalityPercent = 100,
            MaxHealth = MaxHp.baseValue + level * MaxHp.gain[rank],
            MaxHealthPercent = 100,
            Lifesteal = Lifesteal.baseValue + level * Lifesteal.gain[rank],
            LifestealPercent = 100,
            Movespeed = Movespeed.baseValue + level * Movespeed.gain[rank],
            MovespeedPercent = 100,
            AoeResist = AoeResist.baseValue + level * AoeResist.gain[rank],
            AoeResistPercent = 100,
            Block = Block.baseValue + level * Block.gain[rank],
            BlockPercent = 100,
            Evade = Evade.baseValue + level * Evade.gain[rank],
            EvadePercent = 100,
            Parry = Parry.baseValue + level * Parry.gain[rank],
            ParryPercent = 100
        };
    }

    [Serializable]
    public struct Stat {
        public float baseValue;
        public float[] gain;
    }
    public class Manatype {
        public static readonly Manatype MANA = new Manatype("mana", Color.blue);
        public static readonly Manatype RAGE = new Manatype("rage", Color.red);
        public static readonly Manatype ENERGY = new Manatype("energy", Color.yellow);

        private string _name;
        private Color _color;
        public Manatype(string name, Color color) {
            _name = name;
            _color = color;
        }
    }

}