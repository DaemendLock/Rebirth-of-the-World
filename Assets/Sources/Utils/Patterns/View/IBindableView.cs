namespace UtilsUnity.Patterns.View
{
    public interface IBindableView<T>
    {
        void Bind(T viewModel);
    }
}
