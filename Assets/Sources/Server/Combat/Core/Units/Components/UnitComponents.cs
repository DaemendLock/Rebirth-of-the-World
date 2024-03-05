using System.Runtime.CompilerServices;
using Utils.DataTypes;

namespace Core.Combat.Units.Components
{
    public class CastResources
    {
        internal CastResources(float leftMaxValue, float rightMaxValue, ResourceType leftResourceType, ResourceType rightResourceType)
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

        public float GiveResource(ResourceType type, float value)
        {
            if (LeftType == type)
            {
                if (Left.MaxValue - Left.Value < value)
                {
                    value = Left.MaxValue - Left.Value;
                }

                Left += value;
            }
            else
            if (RightType == type)
            {
                if (Right.MaxValue - Right.Value < value)
                {
                    value = Right.MaxValue - Right.Value;
                }

                Right += value;
            }
            else
            {
                return 0;
            }

            return value;
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
}