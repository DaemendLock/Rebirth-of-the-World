using Utils.ByteHelper;

namespace Utils.Interfaces
{
    public interface Value<T>
    {
        T Evaluate();
    }
}
