using Core.Combat.Engine;
using Server.Combat.UserRequests;
using Utils.ByteHelper;

namespace Server.Combat
{
    public enum UserInputType
    {
        Cast,
        Move,
        Stop,
        Attack
    }

    internal class RequestAdapter
    {
        public InputRequest Adapt(ByteReader source)
        {
            byte type = source.ReadByte();

            return (UserInputType) type switch
            {
                UserInputType.Cast => new CastRequest(source.ReadInt(), source.ReadInt(), source.ReadByte()),
                UserInputType.Move => new MoveRequest(source.ReadInt(), new Utils.DataTypes.Vector3(source.ReadFloat(), source.ReadFloat(), source.ReadFloat())),
                UserInputType.Attack => new TargetRequest(source.ReadInt(), source.ReadInt()),
                _ => throw new System.InvalidOperationException(nameof(type)),
            }; ;
        }
    }
}
