namespace Client.Lobby.Infrastructure.Providers
{
    public interface LoadingService<T>
    {
        public T Load(int id);
    }
}
