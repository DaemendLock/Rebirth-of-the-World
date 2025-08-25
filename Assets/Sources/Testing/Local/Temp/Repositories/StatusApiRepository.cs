using System.Collections.Generic;

using Combat.Common.ValueObjects;

using Combat.Local.Domain.API;

namespace Testing.Local.Temp.Factories
{
    public class StatusApiRepository
    {
        private readonly Dictionary<StatusId, StatusApi> _values;

        public StatusApiRepository()
        {
            _values = new();
        }

        public void Create(StatusApi value) => _values.Add(value.Id, value);

        public StatusApi Get(StatusId id) => _values.GetValueOrDefault(id, null);

        public void Delete(StatusId id) => _values.Remove(id);

        public ICollection<StatusApi> GetAll() => _values.Values;
    }
}
