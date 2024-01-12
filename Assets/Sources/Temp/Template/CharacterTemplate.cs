using Data.Characters;
using Data.Characters.Lobby;
using Data.Items;
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
        [SerializeField] private Head _head;
        [SerializeField] private Body _body;
        [SerializeField] private Legs _legs;
        [SerializeField] private Weapon _weaponLeft;
        [SerializeField] private Weapon _weaponRight;
        [SerializeField] private Ring _ring1;
        [SerializeField] private Ring _ring2;
        [SerializeField] private Item _active1;
        [SerializeField] private Item _active2;

        public ItemId[] ToItemIdArray()
        {
            Item[] array = new Item[] {_head, _body, _legs, _weaponRight, _weaponLeft, _ring1, _ring2, _active1, _active2 };

            ItemId[] array2 = new ItemId[array.Length];

            for(int i = 0;i < array.Length;i++)
            {
                if (array[i] == null)
                {
                    array2[i] = (ItemId) (-1);
                    continue;
                }

                array2[i] = array[i].Id;
            }

            return array2;
        }
    }

    [Serializable]
    internal class InventoryItemTemplate
    {
        public int ItemId;
        public int Count;
    }
}
