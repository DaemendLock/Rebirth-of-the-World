using Data.Characters;
using Data.Characters.Lobby;
using System;
using UnityEngine;
using Utils.DataTypes;

namespace Assets.Sources.Temp.Template
{
    [Serializable]
    internal class CharacterDataTemplate
    {
        public int CharId;
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

            return new CharacterState(CharId, ActiveSkin, ActiveSpec,
                new ProgressValue((byte) _level.x, (byte) _level.y, (uint) _level.z, (uint) _level.w),
                 new ProgressValue((byte) _affection.x, (byte) _affection.y, (uint) _affection.z, (uint) _affection.w), ids, Gear.ToItemIdArray());
        }
    }
    public enum GearSlot
    {
        Head,
        Body,
        Legs,
        LeftHand,
        RightHand,
        Ring1,
        Ring2,
        Consumable1,
        Consumable2,
    }

    [Serializable]
    internal class EquipTemplate
    {
        [SerializeField] private int _headId = -1;
        [SerializeField] private int _bodyId = -1;
        [SerializeField] private int _legsId = -1;
        [SerializeField] private int _weaponLeftId = -1;
        [SerializeField] private int _weaponRightId = -1;
        [SerializeField] private int _ring1Id = -1;
        [SerializeField] private int _ring2Id = -1;
        [SerializeField] private int _consume1Id = -1;
        [SerializeField] private int _consume2Id = -1;

        public ItemId[] ToItemIdArray()
        {
            return new ItemId[] { (ItemId) _headId, (ItemId) _bodyId, (ItemId) _legsId, (ItemId) _weaponLeftId, (ItemId) _weaponRightId, (ItemId) _ring1Id, (ItemId) _ring2Id, (ItemId) _consume1Id, (ItemId) _consume2Id };
        }
    }

    [Serializable]
    internal class InventoryItemTemplate
    {
        public int ItemId;
        public int Count;
    }
}
