namespace Combat.API.Objectives
{
    public interface IObjectiveData { }

    public struct ObjectiveInfo<T> where T : unmanaged, IObjectiveData
    {
        public ObjectiveInfo(string name, T data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; }
        public T Data { get; set; }
    }
}
