using System.Collections.Generic;

using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;

using Unity.Collections;

namespace Testing.Local.Temp.Services
{
    public class HealthUpdateService
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IAttributeEvaluationService _attributeEvaluationService;

        public HealthUpdateService(IHealthRepository healthRepository, IAttributeEvaluationService attributeEvaluationService)
        {
            _healthRepository = healthRepository;
            _attributeEvaluationService = attributeEvaluationService;
        }

        public void Update()
        {
            IReadOnlyCollection<Health> oldValues = _healthRepository.GetAll();
            NativeArray<Health> array = new(oldValues.Count, Allocator.Domain, NativeArrayOptions.UninitializedMemory);

            int i = 0;

            foreach (Health health in oldValues)
            {
                array[i++] = health;
            }

            foreach (Health value in array)
            {
                Health health = value;
                health.MaxHealth = value.DefaultHealth + _attributeEvaluationService.GetMaxHealthBonus(health.Id);
                _healthRepository.Update(value);
            }

            array.Dispose();
        }
    }
}
