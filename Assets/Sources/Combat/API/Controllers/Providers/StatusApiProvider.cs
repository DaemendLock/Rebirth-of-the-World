using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.API.Controllers
{
    public class StatusApiProvider
    {
        private readonly Dictionary<StatusId, StatusApi> _values;

        public StatusApiProvider()
        {
            _values = new();
        }

        public void Register(StatusApi value)
        {
            _values.Add(value.Id, value);
        }

        public StatusApi Get(StatusId id) => _values.GetValueOrDefault(id, null);

        public void Delete(StatusId id) => _values.Remove(id);

        public ICollection<StatusApi> GetAll() => _values.Values;
    }
}
