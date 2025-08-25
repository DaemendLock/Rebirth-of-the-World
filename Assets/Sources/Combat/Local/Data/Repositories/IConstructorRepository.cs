using System;
using System.Reflection;

namespace Combat.Local.Data.Repositories
{
    public interface ITypeRepository<in T>
    {
        void Register(Type type);

        bool TryGet(T key, out Type type);
    }

    public interface IConstructorRepository<in T>
    {
        void Register(Type type);

        bool TryGet(T key, out ConstructorInfo constructorInfo);
    }
}
