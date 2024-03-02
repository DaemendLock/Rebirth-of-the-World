namespace Utils.Patterns.Factory
{
    public interface Factory<T>
    {
        T Create();
    }

    public interface Factory<out TResult, in TData> 
    {
        TResult Create(TData data);
    }
}