using Core.Lobby.Characters;
using Data.Items;
using Data.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.DataStructure;

namespace Data.Characters
{
    [CreateAssetMenu(menuName = "Assets/Characters/Character")]
    public class Character : ScriptableObject, Loadable
    {
        private static Dictionary<int, Character> _characters = new Dictionary<int, Character>();

        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private ViewSet[] _veiwSets;
        [SerializeField] private CharacterStats _stats;
        [SerializeField] private CharacterRole[] _roles;
        [SerializeField] private List<WeaponType> _allowedWeapon;

        public int Id => _id;

        public Sprite GetCharacterCard(int activeViewSet) => _veiwSets[activeViewSet].CharacterCard;

        public StatsTable GetStatsTable(int level) => _stats.GetStatsForLevel(level);

        public CharacterRole GetCharacterRole(int activeSpec) => _roles[activeSpec];

        public string LocalizedName => Localization.Localization.GetValue(_name);

        public GameObject GetModel(int activeViewSet) => _veiwSets[activeViewSet].Model;

        public Sprite[] GetSpellIcons(int activeViewSet) => _veiwSets[activeViewSet].GetSpellIcons();

        public void OnLoad()
        {   
            if(_characters.ContainsKey(Id))
            {
                Debug.LogWarning($"Character{_id} overwritten.");
            }

            _characters[_id] = this;
        }

        public static Character Get(int id)
        {
            return _characters[id];
        }

        public static int[] GetLoadedCharactersId()
        {
            return _characters.Keys.ToArray();
        }

        public bool CanHandle(WeaponType type) => _allowedWeapon.Contains(type);
    }
}
