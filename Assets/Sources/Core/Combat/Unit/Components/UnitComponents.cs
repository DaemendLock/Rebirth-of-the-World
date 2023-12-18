using Core.Combat.Engine;
using Core.Combat.Interfaces;
using System.Runtime.CompilerServices;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public class CastResources : CastResourceOwner
    {
        public CastResources(float leftMaxValue, float rightMaxValue, ResourceType leftResourceType, ResourceType rightResourceType)
        {
            Left = new Resource(leftMaxValue);
            Right = new Resource(rightMaxValue);
            LeftType = leftResourceType;
            RightType = rightResourceType;
        }

        public CastResources(UnitCreationData.CastResourceData data) : this(data.LeftResourceMaxValue, data.RightResourceMaxValue,
             data.LeftResourceType, data.RightResourceType)
        { }

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

            Position += MoveDirection * (speed * ModelUpdate.UpdateTime / 1000);
        }
    }
}
