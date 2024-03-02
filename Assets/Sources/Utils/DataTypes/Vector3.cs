#region сборка UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// расположение неизвестно
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Runtime.CompilerServices;
using Utils.ByteHelper;

namespace Utils.DataTypes
{
    public struct Vector3 : IEquatable<Vector3>
    {
        public const float kEpsilon = 1E-05f;

        public const float kEpsilonNormalSqrt = 1E-15f;

        public float x;

        public float y;

        public float z;

        private static readonly Vector3 zeroVector = new Vector3(0f, 0f, 0f);

        private static readonly Vector3 oneVector = new Vector3(1f, 1f, 1f);

        private static readonly Vector3 upVector = new Vector3(0f, 1f, 0f);

        private static readonly Vector3 downVector = new Vector3(0f, -1f, 0f);

        private static readonly Vector3 leftVector = new Vector3(-1f, 0f, 0f);

        private static readonly Vector3 rightVector = new Vector3(1f, 0f, 0f);

        private static readonly Vector3 forwardVector = new Vector3(0f, 0f, 1f);

        private static readonly Vector3 backVector = new Vector3(0f, 0f, -1f);

        private static readonly Vector3 positiveInfinityVector = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        private static readonly Vector3 negativeInfinityVector = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        public float this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    2 => z,
                    _ => throw new IndexOutOfRangeException("Invalid Vector3 index!"),
                };
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }
        }

        public Vector3 normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Normalize(this);
            }
        }

        public float magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (float) Math.Sqrt(x * x + y * y + z * z);
            }
        }

        public float sqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return x * x + y * y + z * z;
            }
        }

        public static Vector3 zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return zeroVector;
            }
        }

        public static Vector3 one
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return oneVector;
            }
        }

        public static Vector3 forward
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return forwardVector;
            }
        }

        public static Vector3 back
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return backVector;
            }
        }

        public static Vector3 up
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return upVector;
            }
        }

        public static Vector3 down
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return downVector;
            }
        }

        public static Vector3 left
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return leftVector;
            }
        }

        public static Vector3 right
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return rightVector;
            }
        }

        public static Vector3 positiveInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return positiveInfinityVector;
            }
        }

        public static Vector3 negativeInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return negativeInfinityVector;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            t = Math.Clamp(t, 0, 1);
            return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            float num = target.x - current.x;
            float num2 = target.y - current.y;
            float num3 = target.z - current.z;
            float num4 = num * num + num2 * num2 + num3 * num3;
            if (num4 == 0f || (maxDistanceDelta >= 0f && num4 <= maxDistanceDelta * maxDistanceDelta))
            {
                return target;
            }

            float num5 = (float) Math.Sqrt(num4);
            return new Vector3(current.x + num / num5 * maxDistanceDelta, current.y + num2 / num5 * maxDistanceDelta, current.z + num3 / num5 * maxDistanceDelta);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3(float x, float y)
        {
            this.x = x;
            this.y = y;
            z = 0f;
        }

        public Vector3(float rotation) : this()
        {
            rotation = rotation * MathF.PI / 180;
            x = MathF.Sin(rotation);
            y = 0;
            z = MathF.Cos(rotation);
        }

        //#if UNITY_2017_1_OR_NEWER
        //        public Vector3(UnityEngine.Vector3 value)
        //        {
        //            x = value.x;
        //            y = value.y;
        //            z = value.z;
        //        }
        //#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Scale(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(Vector3 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
        {
            if (!(other is Vector3))
            {
                return false;
            }

            return Equals((Vector3) other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal)
        {
            float num = -2f * Dot(inNormal, inDirection);
            return new Vector3(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y, num * inNormal.z + inDirection.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Normalize(Vector3 value)
        {
            float num = Magnitude(value);
            if (num > 1E-05f)
            {
                return value / num;
            }

            return zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize()
        {
            float num = Magnitude(this);
            if (num > 1E-05f)
            {
                this /= num;
            }
            else
            {
                this = zero;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Project(Vector3 vector, Vector3 onNormal)
        {
            float num = Dot(onNormal, onNormal);
            if (num < kEpsilon)
            {
                return zero;
            }

            float num2 = Dot(vector, onNormal);
            return new Vector3(onNormal.x * num2 / num, onNormal.y * num2 / num, onNormal.z * num2 / num);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
        {
            float num = Dot(planeNormal, planeNormal);
            if (num < kEpsilon)
            {
                return vector;
            }

            float num2 = Dot(vector, planeNormal);
            return new Vector3(vector.x - planeNormal.x * num2 / num, vector.y - planeNormal.y * num2 / num, vector.z - planeNormal.z * num2 / num);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(Vector3 from, Vector3 to)
        {
            float num = (float) Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            if (num < 1E-15f)
            {
                return 0f;
            }

            float num2 = Math.Clamp(Dot(from, to) / num, -1f, 1f);
            return (float) Math.Acos(num2) * 57.29578f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
        {
            float num = Angle(from, to);
            float num2 = from.y * to.z - from.z * to.y;
            float num3 = from.z * to.x - from.x * to.z;
            float num4 = from.x * to.y - from.y * to.x;
            float num5 = Math.Sign(axis.x * num2 + axis.y * num3 + axis.z * num4);
            return num * num5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector3 a, Vector3 b)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;
            float num3 = a.z - b.z;
            return (float) Math.Sqrt(num * num + num2 * num2 + num3 * num3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ClampMagnitude(Vector3 vector, float maxLength)
        {
            float num = vector.sqrMagnitude;
            if (num > maxLength * maxLength)
            {
                float num2 = (float) Math.Sqrt(num);
                float num3 = vector.x / num2;
                float num4 = vector.y / num2;
                float num5 = vector.z / num2;
                return new Vector3(num3 * maxLength, num4 * maxLength, num5 * maxLength);
            }

            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Magnitude(Vector3 vector)
        {
            return (float) Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrMagnitude(Vector3 vector)
        {
            return vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Min(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Max(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(0f - a.x, 0f - a.y, 0f - a.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Vector3 a, float d)
        {
            return new Vector3(a.x * d, a.y * d, a.z * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(float d, Vector3 a)
        {
            return new Vector3(a.x * d, a.y * d, a.z * d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.y * b.z - a.z - b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator /(Vector3 a, float d)
        {
            return new Vector3(a.x / d, a.y / d, a.z / d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            float num = lhs.x - rhs.x;
            float num2 = lhs.y - rhs.y;
            float num3 = lhs.z - rhs.z;
            float num4 = num * num + num2 * num2 + num3 * num3;
            return num4 < 9.99999944E-11f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return $"{x}, {y}, {z}";
        }

        public static Vector3 Parse(ByteReader source)
        {
            float xComponent = source.ReadFloat();
            float yComponent = source.ReadFloat();
            float zComponent = source.ReadFloat();

            return new Vector3(xComponent, yComponent, zComponent);
        }
    }
}
