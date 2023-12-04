using Core.Lobby.Characters;
using Data.Characters.Lobby;
using System;
using UnityEngine;
using Utils.DataTypes;

namespace Assets.Sources.Temp.Template
{
    [Serializable]
    internal class CharacterDataTemplate
    {
        public int charId;
        public int ActiveSkin;
        public int ActiveSpec;

        [SerializeField] public Vector4 _level;
        [SerializeField] public Vector4 _affection;

        public int[] Spells = new int[6];
        public EquipTemplate Gear;

        public CharacterState GetCharacterData()
        {
            SpellId[] ids = new SpellId[Spells.Length];

            for (int i = 0; i < Spells.Length; i++)
            {
                ids[i] = new SpellId(Spells[i]);
            }

            return new CharacterState(
                new ProgressValue((byte) _level.x, (byte) _level.y, (uint) _level.z, (uint) _level.w),
                 new ProgressValue((byte) _affection.x, (byte) _affection.y, (uint) _affection.z, (uint) _affection.w), ids, Gear.ToItemIdArray());
        }
    }
}
