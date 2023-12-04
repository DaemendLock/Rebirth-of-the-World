using System;
using UnityEngine;
using Utils.DataTypes;

namespace Assets.Sources.Temp.Template
{
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
}
