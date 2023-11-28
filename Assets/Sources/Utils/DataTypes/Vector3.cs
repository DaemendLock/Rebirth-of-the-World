#region сборка UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// расположение неизвестно
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Runtime.CompilerServices;

namespace Utils.DataTypes
{
    public struct Vector3 : IEquatable<Vector3>
    {
        public const float kEpsilon = 1E-05f;

        public const float kEpsilonNormalSqrt = 1E-15f;

        //
        // Сводка:
        //     X component of the vector.
        public float x;

        //
        // Сводка:
        //     Y component of the vector.
        public float y;

        //
        // Сводка:
        //     Z component of the vector.
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

        //
        // Сводка:
        //     Returns this vector with a magnitude of 1 (Read Only).
        public Vector3 normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Normalize(this);
            }
        }

        //
        // Сводка:
        //     Returns the length of this vector (Read Only).
        public float magnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (float) Math.Sqrt(x * x + y * y + z * z);
            }
        }

        //
        // Сводка:
        //     Returns the squared length of this vector (Read Only).
        public float sqrMagnitude
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return x * x + y * y + z * z;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3(0, 0, 0).
        public static Vector3 zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return zeroVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3(1, 1, 1).
        public static Vector3 one
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return oneVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3(0, 0, 1).
        public static Vector3 forward
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return forwardVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3(0, 0, -1).
        public static Vector3 back
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return backVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3(0, 1, 0).
        public static Vector3 up
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return upVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3(0, -1, 0).
        public static Vector3 down
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return downVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3(-1, 0, 0).
        public static Vector3 left
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return leftVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3(1, 0, 0).
        public static Vector3 right
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return rightVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity,
        //     float.PositiveInfinity).
        public static Vector3 positiveInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return positiveInfinityVector;
            }
        }

        //
        // Сводка:
        //     Shorthand for writing Vector3(float.NegativeInfinity, float.NegativeInfinity,
        //     float.NegativeInfinity).
        public static Vector3 negativeInfinity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return negativeInfinityVector;
            }
        }

        //
        // Сводка:
        //     Linearly interpolates between two points.
        //
        // Параметры:
        //   a:
        //     Start value, returned when t = 0.
        //
        //   b:
        //     End value, returned when t = 1.
        //
        //   t:
        //     Value used to interpolate between a and b.
        //
        // Возврат:
        //     Interpolated value, equals to a + (b - a) * t.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            t = Math.Clamp(t, 0, 1);
            return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        //
        // Сводка:
        //     Linearly interpolates between two vectors.
        //
        // Параметры:
        //   a:
        //
        //   b:
        //
        //   t:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        //
        // Сводка:
        //     Calculate a position between the points specified by current and target, moving
        //     no farther than the distance specified by maxDistanceDelta.
        //
        // Параметры:
        //   current:
        //     The position to move from.
        //
        //   target:
        //     The position to move towards.
        //
        //   maxDistanceDelta:
        //     Distance to move current per call.
        //
        // Возврат:
        //     The new position.
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

        //
        // Сводка:
        //     Creates a new vector with given x, y, z components.
        //
        // Параметры:
        //   x:
        //
        //   y:
        //
        //   z:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        //
        // Сводка:
        //     Creates a new vector with given x, y components and sets z to zero.
        //
        // Параметры:
        //   x:
        //
        //   y:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3(float x, float y)
        {
            this.x = x;
            this.y = y;
            z = 0f;
        }

        //
        // Сводка:
        //     Creates a new vector with given roation(in degree) and sets y to zero
        public Vector3(float rotation) : this()
        {
            rotation = rotation * MathF.PI / 180;
            x = MathF.Sin(rotation);
            y = 0;
            z = MathF.Cos(rotation);
        }

        //
        // Сводка:
        //     Set x, y and z components of an existing Vector3.
        //
        // Параметры:
        //   newX:
        //
        //   newY:
        //
        //   newZ:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        //
        // Сводка:
        //     Multiplies two vectors component-wise.
        //
        // Параметры:
        //   a:
        //
        //   b:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Scale(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        //
        // Сводка:
        //     Multiplies every component of this vector by the same component of scale.
        //
        // Параметры:
        //   scale:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(Vector3 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        //
        // Сводка:
        //     Cross Product of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
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

        //
        // Сводка:
        //     Returns true if the given vector is exactly equal to this vector.
        //
        // Параметры:
        //   other:
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

        //
        // Сводка:
        //     Reflects a vector off the plane defined by a normal.
        //
        // Параметры:
        //   inDirection:
        //
        //   inNormal:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal)
        {
            float num = -2f * Dot(inNormal, inDirection);
            return new Vector3(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y, num * inNormal.z + inDirection.z);
        }

        //
        // Сводка:
        //     Makes this vector have a magnitude of 1.
        //
        // Параметры:
        //   value:
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

        //
        // Сводка:
        //     Dot Product of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        //
        // Сводка:
        //     Projects a vector onto another vector.
        //
        // Параметры:
        //   vector:
        //
        //   onNormal:
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

        //
        // Сводка:
        //     Projects a vector onto a plane defined by a normal orthogonal to the plane.
        //
        // Параметры:
        //   planeNormal:
        //     The direction from the vector towards the plane.
        //
        //   vector:
        //     The location of the vector above the plane.
        //
        // Возврат:
        //     The location of the vector on the plane.
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

        //
        // Сводка:
        //     Calculates the angle between vectors from and.
        //
        // Параметры:
        //   from:
        //     The vector from which the angular difference is measured.
        //
        //   to:
        //     The vector to which the angular difference is measured.
        //
        // Возврат:
        //     The angle in degrees between the two vectors.
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

        //
        // Сводка:
        //     Calculates the signed angle between vectors from and to in relation to axis.
        //
        // Параметры:
        //   from:
        //     The vector from which the angular difference is measured.
        //
        //   to:
        //     The vector to which the angular difference is measured.
        //
        //   axis:
        //     A vector around which the other vectors are rotated.
        //
        // Возврат:
        //     Returns the signed angle between from and to in degrees.
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

        //
        // Сводка:
        //     Returns the distance between a and b.
        //
        // Параметры:
        //   a:
        //
        //   b:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector3 a, Vector3 b)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;
            float num3 = a.z - b.z;
            return (float) Math.Sqrt(num * num + num2 * num2 + num3 * num3);
        }

        //
        // Сводка:
        //     Returns a copy of vector with its magnitude clamped to maxLength.
        //
        // Параметры:
        //   vector:
        //
        //   maxLength:
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

        //
        // Сводка:
        //     Returns a vector that is made from the smallest components of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Min(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
        }

        //
        // Сводка:
        //     Returns a vector that is made from the largest components of two vectors.
        //
        // Параметры:
        //   lhs:
        //
        //   rhs:
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

        //
        // Сводка:
        //     Returns a formatted string for this vector.
        //
        // Параметры:
        //   format:
        //     A numeric format string.
        public override string ToString()
        {
            return $"{x}, {y}, {z}";
        }

        public static Vector3 Parse(byte[] source, int start)
        {
            float xComponent = BitConverter.ToSingle(source, start);
            float yComponent = BitConverter.ToSingle(source, start + sizeof(float));
            float zComponent = BitConverter.ToSingle(source, start + sizeof(float) * 2);

            return new Vector3(xComponent, yComponent, zComponent);
        }
    }
}
