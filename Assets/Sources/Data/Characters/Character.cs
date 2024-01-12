using Core.Lobby.Characters;
using Data.Entities;
using Data.Items;
using Data.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using Utils.DataStructure;
using Utils.DataTypes;

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

        public int Id => _id;
        public string LocalizedName => Localization.Localization.GetValue(_name);

        public bool CanHandle(WeaponType type) => _allowedWeapon.Contains(type);

        public UnitCreationData.CastResourceData CastResources => _npc.CastResources;

        public CharacterRole GetCharacterRole(int activeSpec) => _roles[activeSpec];

        public Sprite GetCharacterCard() => _npc.GetCharacterCard();

        public StatsTable GetStatsTable(int level) => _npc.GetStatsTable(level);

        public NpcModel GetModel() => _npc.GetModel();

        public Sprite[] GetSpellIcons() => _npc.GetSpellIcons();

        public AnimatorController GetAnimatorController() => _npc.GetAnimatorController();

        public void OnLoad()
        {
            if (_characters.ContainsKey(Id))
            {
                Debug.LogWarning($"Character{_id} overwritten.");
            }

            _characters[_id] = this;
        }

        public UnitCreationData GetUnitCreationData(int index, byte team, CharacterState state, byte contolGroup) => _npc.GetUnitCreationData(index, team, state, contolGroup);

        public static Character Get(int id)
        {
            return _characters[id];
        }

        public static int[] GetLoadedCharactersId()
        {
            return _characters.Keys.ToArray();
        }
    }
}
