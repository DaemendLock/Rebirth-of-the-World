using System.IO;

namespace Utils.Interfaces
{
    public interface SerializableInterface
    {
        void Serialize(BinaryWriter buffer);
    }

    public interface Value<T>
    {
        T Evaluate();
    }
}
