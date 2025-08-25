using System.Collections.Generic;

using Client.Combat.Presentation;
using Client.Combat.Presentation.Units;
using Client.Testing.View;

using Server.Combat.Data.Repositories;
using Server.Combat.Domain.Repositories;
using Server.Combat.Domain.Services;
using Server.Combat.Domain.Units.Services.Implementations;
using Server.Combat.Domain.Units.ValueObjects;
using Server.Combat.Infrastructure.Factories;
using Server.Combat.Infrastructure.Implementations.Factories;
using Server.Combat.Infrastructure.Implementations.Repositories;
using Server.Combat.Infrastructure.Implementations.Services;
using Server.Combat.Infrastructure.Repositories;
using Server.Combat.Infrastructure.Services;

using UnityEngine;

using Zenject;

namespace Assets.Sources.Common
{
    public class AssetProvider : MonoBehaviour
    {
        [SerializeField] private GameObject _testModel;

        [field: SerializeField] public Nameplate NameplatePrefab { get; private set; }

        public GameObject GetUnitModel(int modelId) => _testModel;
    }

    public partial class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
            BindFactories();
            BindRepositories();

            //Container.Bind<AssetProvider>().FromComponentsOn(gameObject).AsSingle();
            Container.Bind<Client.Combat.Infrastructure.Controllers.ICombatController>().To<Client.Combat.Infrastructure.Implementations.Controllers.CombatController>().AsSingle();
            Container.Bind<Server.Combat.Infrastructure.Controllers.ICombatController>().To<Server.Combat.Infrastructure.Implementations.Controllers.CombatController>().AsSingle();
            Container.Bind<Temp.ClientInputReader>().FromNew().AsSingle();
            Container.Bind<IEnumerable<KeyValuePair<EntityId, Server.Combat.Infrastructure.Controllers.IUnitController>>>().To<UnitControllerRepository>().FromResolve();
            Container.Bind<ITestMenuStrategy>().To<TestMenuStrategy>().AsSingle();
            Container.Bind<ICameraView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Client.Combat.Infrastructure.Controllers.ICameraController>().To<Client.Combat.Infrastructure.Implementations.Controllers.CameraController>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<ISkillCastService>().To<SkillCastService>().AsSingle();
            Container.Bind<IHealDamageApplicationService>().To<HealDamageApplicationService>().AsSingle();
        }

        private void BindRepositories()
        {
            Container.Bind<IStatusRepository>().To<StatusRepository>().AsSingle();
            //Container.Bind<IPositionRepository>().To<PositionRepository>().AsSingle();
            Container.Bind<IActionHandlerRepository>().To<ActionHandlerRepository>().AsSingle();
            Container.Bind<IHitboxesRepository>().To<HitboxesRepository>().AsSingle();
            //Container.Bind<IFrameDataRepository>().To<FrameDataRepository>().AsSingle();
            Container.Bind<IAttributesRepository>().To<AttributesRepository>().AsSingle();
            Container.Bind<IUnitControllerRepository>().To<UnitControllerRepository>().AsSingle();
            Container.Bind<ISkillDataRepository>().To<SkillDataRepository>().AsSingle();
            Container.Bind<ISkillRepository>().To<SkillRepository>().AsSingle();
            Container.Bind<ISkillScriptConstructorRepository>().To<SkillConstructorRepository>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IUnitModelFactory>().To<UnitModelFactory>().AsSingle();
            Container.Bind<ISkillScriptFactory>().To<ReflectionSkillScriptFactory>().AsSingle();
            Container.Bind<ISkillFactory>().To<SkillFactory>().AsSingle();
            Container.Bind<IUnitControllerFactory>().To<UnitControllerFactory>().AsSingle();
        }
    }
}