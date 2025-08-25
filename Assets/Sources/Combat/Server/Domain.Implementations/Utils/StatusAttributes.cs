namespace Server.Combat.Domain.Implementations.Statuses.Attributes
{
    public class DestroyOnExpireAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AuraIdAttribute : System.Attribute
    {
        public AuraIdAttribute(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class StatusNameAttribute : System.Attribute
    {
        public StatusNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
