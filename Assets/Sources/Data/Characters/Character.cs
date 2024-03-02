using Core.Lobby.Characters;
using Data.Animations;
using Data.Entities;
using Data.Items;
using Data.Sounds;
using Data.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Characters
{
    [CreateAssetMenu(menuName = "Assets/Characters/Character")]
    public class Character : ScriptableObject, Loadable
    {
        private static Dictionary<int, Character> _characters = new Dictionary<int, Character>();

        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Npc _npc;
        [SerializeField] private CharacterRole[] _roles;
        [SerializeField] private List<WeaponType> _allowedWeapon;
        [SerializeField] private List<Specialization> _specializations;

        public static Dictionary<int, Character>.ValueCollection Values => _characters.Values;

        public int Id { get => _id; }
        public string Name { get => _name; set => _name = value; }
        public Npc Npc { get => _npc; set => _npc = value; }
        public CharacterRole[] Roles { get => _roles; set => _roles = value; }
        public List<WeaponType> AllowedWeapon { get => _allowedWeapon; set => _allowedWeapon = value; }

        public void OnLoad()
        {
            if (_characters.ContainsKey(_id))
            {
                Debug.LogWarning($"Character{_id} overwritten.");
            }

            _characters[_id] = this;
        }

        public Sprite GetCardSprite(int viewSet) => _npc.GetCharacterCard(viewSet);

        public int[] GetSpells(int activeSpec) => _specializations[activeSpec].Spells;

        public static Character Get(int id)
        {
            return _characters[id];
        }
    }

    [Serializable]
    public class Specialization
    {
        public int[] Spells;
    }
}
