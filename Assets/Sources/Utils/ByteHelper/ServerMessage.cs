namespace UtilsUnity.ByteHelper
{
    public readonly struct ServerMessage
    {
        public readonly int Id;
        public readonly byte[] Data;

        public ServerMessage(int id, byte[] data)
        {
            Id = id;
            Data = data;
        }
    }
}
