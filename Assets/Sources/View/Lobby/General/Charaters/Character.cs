using Core.Lobby.Characters;
using UnityEngine;
using View.Lobby.Utils;

namespace View.Lobby.General.Charaters
{
    public class Character
    {
        public readonly string NameToken;
        public readonly GameObject Model;
        public readonly SpriteProvider Picture;

        public readonly CharacterRole Role;

        public Character(string nameToken, GameObject model, SpriteProvider picture, CharacterRole role)
        {
            NameToken = nameToken;
            Model = model;
            Picture = picture;
            Role = role;
            //_status = status;
        }

        //public ProgressValue Level => _status.Level;

        //public ProgressValue Affection => _status.Affection;

        //public SpellId[] Spells => _status.Spells;

        //public ItemId[] Gear => _status.Gear;

        //public StatsTable EvaluateStats()
        //{
        //    return _status.EvaluateStats();
        //}

        //public void UpdateData(CharacterData data)
        //{
        //    _status = data;
        //}
    }
}
