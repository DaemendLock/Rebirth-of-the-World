using Core.Combat.Interfaces;
using Core.Combat.Items.Gear;
using System;
using System.Runtime.CompilerServices;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public struct Resource
    {
        private float _value;
        private float _minValue;
        private float _maxValue;

        public Resource(float maxValue)
        {
            _maxValue = maxValue;
            _minValue = 0;
            _value = maxValue;
        }

        public Resource(float minValue, float maxValue)
        {
            _maxValue = maxValue;
            _minValue = minValue;
            _value = maxValue;
        }

        public float Value
        {
            get => _value;
            set => _value = Math.Clamp(value, MinValue, MaxValue);
        }
        public float MinValue { get => _minValue; set => Math.Max(0, value); }
        public float MaxValue { get => _maxValue; set => Math.Max(MinValue, value); }

        public static Resource operator +(Resource resource, float value)
        {
            Resource result = resource;

            result.Value += value;

            if (result.Value < resource.MinValue)
            {
                result.Value = resource.MinValue;
            }
            else if (result.Value > result.MaxValue)
            {
                result.Value = result.MaxValue;
            }

            return result;
        }

        public static Resource operator -(Resource resource, float value)
        {
            Resource result = resource;
            result.Value -= value;

            if (result.Value < result.MinValue)
            {
                result.Value = result.MinValue;
            }
            else if (result.Value > result.MaxValue)
            {
                result.Value = result.MaxValue;
            }

            return result;
        }

        public static bool operator >(Resource resource, float value)
        {
            return resource.Value > value;
        }

        public static bool operator >=(Resource resource, float value)
        {
            return resource.Value >= value;
        }

        public static bool operator <(Resource resource, float value)
        {
            return resource.Value < value;
        }

        public static bool operator <=(Resource resource, float value)
        {
            return resource.Value <= value;
        }

        public static bool operator ==(Resource resource, float value)
        {
            return Math.Abs(resource.Value - value) < 0.001;
        }

        public static bool operator !=(Resource resource, float value)
        {
            return Math.Abs(resource.Value - value) > 0.001;
        }

        public override string ToString()
        {
            return $"Resource:({_value}/{_minValue}-{_maxValue})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Resource resource &&
                   _value == resource._value &&
                   _minValue == resource._minValue &&
                   _maxValue == resource._maxValue;
        }
    }

    public class CastResources : CastResourceOwner
    {
        public CastResources(float leftMaxValue, float rightMaxValue, ResourceType leftResourceType, ResourceType rightResourceType)
        {
            Left = new Resource(leftMaxValue);
            Right = new Resource(rightMaxValue);
            LeftType = leftResourceType;
            RightType = rightResourceType;
        }

        public Resource Left { get; private set; }
        public ResourceType LeftType { get; private set; }

        public Resource Right { get; private set; }
        public ResourceType RightType { get; private set; }

        public void SpendResource(AbilityCost cost)
        {
            Left -= cost.Left;
            Right -= cost.Right;
        }

        public void GiveResource(ResourceType type, float value)
        {
            if (LeftType == type)
            {
                Left += value;
            }

            if (RightType == type)
            {
                Right += value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanPay(AbilityCost value)
        {
            return Left >= value.Left && Right >= value.Right;
        }

        public bool HasResource(ResourceType type) => LeftType == type || RightType == type;

        public float GetResourceValue(ResourceType type)
        {
            if (LeftType == type)
            {
                return Left.Value;
            }

            if (RightType == type)
            {
                return Right.Value;
            }

            return 0;
        }

        internal void Update(float left, float right)
        {
            Left += left;
            Right += right;
        }
    }

    public class PositionComponent
    {
        public Vector3 Position { get; set; }

        public Vector3 MoveDirection { get; set; }

        public bool IsMoving { get; set; }

        public float Rotation { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(float speed)
        {
            if (IsMoving == false)
            {
                return;
            }

            Position += MoveDirection * (speed * Engine.Combat.UpdateTime / 1000);
        }
    }

    public class Equipment
    {
        private CombatGear[] _gear = new CombatGear[(int) GearSlot.CONSUMABLE_2 + 1];

        public bool CanEquip(CombatGear gear)
        {
            if (_gear[(int) gear.Slot] != null)
            {
                return false;
            }

            if (gear.RestrictOffhand && _gear[(int) GearSlot.OFF_HAND] != null)
            {
                return false;
            }

            if (gear.Slot == GearSlot.OFF_HAND && _gear[(int) GearSlot.MAIN_HAND] != null && _gear[(int) GearSlot.MAIN_HAND].RestrictOffhand)
            {
                return false;
            }

            return true;
        }

        public void Equip(CombatGear gear)
        {
            if (CanEquip(gear) == false)
            {
                throw new InvalidOperationException("Can't equip " + gear);
            }

            _gear[(int) gear.Slot] = gear;
        }

        public CombatGear GetItemInSlot(GearSlot slot)
        {
            return _gear[(int) slot];
        }

        public CombatGear Unequip(GearSlot slot)
        {
            CombatGear result = _gear[(int) slot];
            _gear[(int) slot] = null;
            return result;
        }
    }
}
