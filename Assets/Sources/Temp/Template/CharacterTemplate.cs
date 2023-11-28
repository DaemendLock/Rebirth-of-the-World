using Core.Lobby.Characters;
using Data.Characters.Lobby;
using UnityEngine;
using Utils.DataTypes;
using View.Lobby.General.Charaters;
using View.Lobby.Utils;

namespace Temp.Template
{
    internal class CharacterTemplate : MonoBehaviour
    {
        [SerializeField] public GameObject _model;
        [SerializeField] public Sprite _sprite;
        [SerializeField] public CharacterRole _role;

        [SerializeField] public string _name;
        [SerializeField] public Vector4 _level;
        [SerializeField] public Vector4 _affection;

        [SerializeField] public int[] _spellIds;

        [SerializeField] public int _id;

        private void Start()
        {
            SpellId[] ids = new SpellId[_spellIds.Length];

            for (int i = 0; i < _spellIds.Length; i++)
            {
                ids[i] = new SpellId(_spellIds[i]);
            }

            CharacterData data = new CharacterData(
                new ProgressValue((byte) _level.x, (byte) _level.y, (uint) _level.z, (uint) _level.w),
                 new ProgressValue((byte) _affection.x, (byte) _affection.y, (uint) _affection.z, (uint) _affection.w), ids);

            SpriteProvider sprite = new StaticSprite(_sprite);

            CharactersList.RegisterCharacter(_id, new Character(_name, _model, sprite, _role));
        }
    }
}
