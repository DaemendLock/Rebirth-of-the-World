using Combat.Common.ValueObjects;
using Combat.Local.Data.Repositories;
using Combat.Local.Data.Services;
using Combat.Local.Domain.API.IDK;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;
using Combat.Local.Factories;
using Combat.Local.Infrastructure.Controllers;
using Combat.Local.Infrastructure.Factories;
using Combat.Server.Data.Repositories.Unit;

using Temp.Domain.Implementations;
using Temp.Repositories.Implementations;

using Testing.Local.Temp.Factories;
using Testing.Local.Temp.Services;

using Zenject;

namespace Testing.Local
{
    public partial class LocalSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindRepositories();
            BindServices();
            BindFactories();

            //Container.Bind<AssetProvider>().FromComponentsOn(gameObject).AsSingle();
            Container.Bind<CombatController>().FromNew().AsSingle();
            Container.Bind<ITickable>().To<CombatController>().FromResolve();

            Container.Resolve<ISkillRegistrationService>();

            var val = Container.Resolve<CombatController>();
            StatsTable stats = StatsTable.UnitDefault;
            stats[Combat.Local.Domain.ValueObjects.Attribute.Speed] = new(1, 100);
            val.CreateUnit(new(new("Katerina"), 2, null, new(5, 5, 5), 1000, 1000, stats, new SkillId[] { new(0)}));
        }

        private void BindRepositories()
        {
            Container.Bind<ISkillRepository>().To<SkillRepository>().AsSingle();
            Container.Bind<IStatusRepository>().To<StatusRepository>().AsSingle();
            Container.Bind<IHealthRepository>().To<HealthRepository>().AsSingle();
            Container.Bind<IKillableRepository>().To<KillableRepository>().AsSingle();
            Container.Bind<IPositionRepository>().To<PositionRepository>().AsSingle();
            Container.Bind<IActionRepository>().To<CastActionRepository>().AsSingle();
            Container.Bind<ISkillAnimationRepository>().To<SkillAnimationRepository>().AsSingle();
            Container.Bind<ISkillScriptNameRepository>().To<SkillScriptNameRepository>().AsSingle();
            Container.Bind<IAttributesRepository>().To<AttributesRepository>().AsSingle();
            Container.Bind<ICastableRepository>().To<CastableRepository>().AsSingle();
            Container.Bind<IResourceRepository>().To<ResourceRepository>().AsSingle();
            Container.Bind<IHitboxRepository>().To<HitboxRepository>().AsSingle();
            Container.Bind<IHurtableRepository>().To<HurtableRepository>().AsSingle();

            Container.Bind<CharacterModelRepository>().FromNew().AsSingle();

            Container.Bind<UnitApiRepository>().FromNew().AsSingle();
            Container.Bind<SkillApiRepository>().FromNew().AsSingle();
            Container.Bind<StatusApiRepository>().FromNew().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<HealthService>().FromNew().AsSingle();
            Container.Bind<IKillReviveService>().To<KillReviveService>().AsSingle();
            Container.Bind<IModelUpdateService>().To<ModelUpdateService>().AsSingle();
            Container.Bind<ISkillCastService>().To<SkillCastService>().AsSingle();
            Container.Bind<IStatusService>().To<StatusService>().AsSingle();
            Container.Bind<ISkillRegistrationService>().To<SkillRegistrationService>().AsSingle();
            Container.Bind<IAttributeEvaluationService>().To<AttributeEvaluationService>().AsSingle();
            Container.Bind<IMovementService>().To<MovementService>().AsSingle();
            Container.Bind<IHitHandlingService>().To<HitHandlingService>().AsSingle();
            Container.Bind<IStatusLookupService>().To<StatusLookupService>().AsSingle();

            Container.Bind<AttributeUpdateService>().FromNew().AsSingle();
            Container.Bind<HealthUpdateService>().FromNew().AsSingle();
            Container.Bind<ResourceService>().FromNew().AsSingle();
            Container.Bind<StatusUpdateService>().FromNew().AsSingle();

            Container.Bind<StatusApiUpdateService>().FromNew().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<HitController>().FromNew().AsSingle();
            Container.Bind<IUnitModelFactory>().To<UnitModelFactory>().AsSingle();
            Container.Bind<IUnitViewFactory>().To<UnitViewFactory>().AsSingle();
            Container.Bind<IUnitControllerFactory>().To<UnitControllerFactory>().AsSingle();
            Container.Bind<IStatusFactory>().To<StatusFactory>().AsSingle();

            Container.Bind<SkillApiFactory>().FromNew().AsSingle();
            Container.Bind<UnitApiFactory>().FromNew().AsSingle();
            Container.Bind<StatusApiFactory>().FromNew().AsSingle();
        }
    }
}