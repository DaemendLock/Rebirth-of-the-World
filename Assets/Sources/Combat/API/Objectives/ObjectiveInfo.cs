namespace Combat.API.Objectives
{
    public interface IObjectiveData { }

    public struct ObjectiveInfo<T> where T : unmanaged, IObjectiveData
    {
        public ObjectiveInfo(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
