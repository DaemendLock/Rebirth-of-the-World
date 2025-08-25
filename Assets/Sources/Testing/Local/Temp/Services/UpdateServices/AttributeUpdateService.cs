using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.Entities;
using System;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.API;
using Testing.Local.Temp.Factories;

namespace Testing.Local.Temp.Services
{
    public interface IUpdateService
    {
        int Priority { get; }
        void Update();
    }

    public class AttributeUpdateService
    {
        private readonly IAttributesRepository _attrubuteRepository;
        private readonly IStatusLookupService _statusLookupService;
        private readonly StatusApiRepository _statusApiRepository;
        private readonly AttributeValue[] _buffer;

        public AttributeUpdateService(IAttributesRepository attrubuteRepository, IStatusLookupService statusLookupService, StatusApiRepository statusApiRepository)
        {
            _attrubuteRepository = attrubuteRepository;
            _statusLookupService = statusLookupService;
            _statusApiRepository = statusApiRepository;

            _buffer = new AttributeValue[Attributes.AttributeCount];
        }

        public void Update()
        {
            lock (_buffer)
            {
                foreach (Attributes attributes in _attrubuteRepository.GetAll())
                {
                    Array.Clear(_buffer, 0, Attributes.AttributeCount);
                    AttributesData data = new(attributes, _buffer);

                    //_api.GetAttributeModifications(data);
                    IAttributesModifier modifier;

                    foreach (StatusApi value in _statusLookupService.FindStatusesOnUnit(attributes.Id))
                    {
                        if (value.Update() == false)
                        {
                            _statusApiRepository.Delete(value.Id);
                        }

                        if (value.TryGetProperty(out modifier) == false)
                        {
                            continue;
                        }

                        modifier.ModifyAttributes(data);
                    }

                    attributes.WriteBonues(_buffer);
                }
            }
        }
    }
}
