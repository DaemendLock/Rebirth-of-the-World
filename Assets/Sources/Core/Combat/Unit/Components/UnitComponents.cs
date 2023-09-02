using Core.Combat.Abilities;
using Core.Combat.Interfaces;
using Core.Combat.Items.Gear;
using Core.Combat.Utils;
using System;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public struct Resource
    {
        private float _value;
        private float _minValue;
        private float _maxValue;

        public Resource(int maxValue)
        {
            _maxValue = maxValue;
            _minValue = 0;
            _value = maxValue;
        }

        public Resource(int minValue, int maxValue)
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

            if (result.Value < 0)
            {
                result.Value = 0;
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

            if (result.Value < 0)
            {
                result.Value = 0;
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
    }

    public struct CastResources : CastResourceOwner
    {
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
    }

    public class PositionComponent
    {
        public Position Position;

        private Vector3 _moveDirection;

        public Vector3 MoveDirection
        {
            get => _moveDirection;
            set
            {
                _moveDirection = value;
                //Moving = value.magnitude > 0;
            }
        }

        public bool Moving { get; private set; }

        public void EvaluateNextLocation(float speed, float time)
        {
            //TODO
            //Position.Location += _moveDirection * speed * time;
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
