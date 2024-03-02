namespace Utils.Patterns.Adapters
{
    public interface Adapter<out Target, in Adaptee>
    {
        Target Adapt(Adaptee value);
    }
}
