using System.IO;

namespace Utils.Interfaces
{
    public interface SerializableInterface
    {
        void Serialize(BinaryWriter buffer);
    }
}
