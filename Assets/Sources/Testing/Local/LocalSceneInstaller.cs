using Combat.API;
using Combat.API.Controllers;
using Combat.Common.ValueObjects;
using Combat.Local.Controllers;
using Combat.Local.Data.Databases;
using Combat.Local.Data.Factories;
using Combat.Local.Data.Lookup;
using Combat.Local.Data.Presentation;
using Combat.Local.Data.Repositories;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Scene;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Repositories;
using Combat.Local.Gateways.Repositories.Unit;
using Combat.Local.Presentation.Factories;
using Combat.Local.Presentation.Presenters;

using Temp.Domain.Implementations;

using Testing.Local.Temp.DomainOutputs;
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
            BindUseCases();
            BindDataSources();
            BindControllers();

            //Container.Bind<AssetProvider>().FromComponentsOn(gameObject).AsSingle();

            Container.Bind<ScenePresenter>().FromNew().AsSingle();
            Container.Bind<UnitPresenter>().FromNew().AsSingle();
            Container.Bind<SceneApi>().FromNew().AsSingle();
            Container.Bind<SceneApiProvider>().FromNew().AsSingle();

            var val = Container.Resolve<CombatController>();
            Container.Resolve<SceneApiProvider>().Register(Container.Resolve<SceneApi>());
            Container.Resolve<SkillDataBase>();

            StatsTable stats = StatsTable.UnitDefault;
            stats[Attribute.Speed] = new(1, 100);
            System.Span<SkillId> skills = stackalloc SkillId[] { new(0) };
            val.CreateUnit(new(new("Katerina"), new(2), new(5, 5, 5), 1000, 1000, stats.ToAttributeArray(), skills));
        }

        private void BindControllers()
        {
            Container.Bind<CombatController>().FromNew().AsSingle();
            Container.Bind<CharacterController>().FromNew().AsSingle();
            Container.Bind<PlayerController>().FromNew().AsSingle();
            Container.Bind<StatusController>().FromNew().AsSingle();
            Container.Bind<HitController>().FromNew().AsSingle();
            Container.Bind<HealthOwnerController>().FromNew().AsSingle();
            Container.Bind<AttributeOwnerController>().FromNew().AsSingle();

            Container.Bind<UnitEventApiController>().FromNew().AsSingle();
        }

        private void BindDataSources()
        {
            Container.Bind<SkillDataBase>().FromNew().AsSingle();
            Container.Bind<ISkillDataBase>().To<SkillDataBase>().FromResolve();

            Container.Bind<CharacterViewContainer>().FromNew().AsSingle();
            Container.Bind<ICharacterViewContainer>().To<CharacterViewContainer>().FromResolve();
            Container.Bind<ISceneObjectDataSource>().To<CharacterViewContainer>().FromResolve();

            Container.Bind<StatusModificationProvider>().FromNew().AsSingle();
            Container.Bind<IStatusApiDataSource>().To<StatusModificationProvider>().FromResolve();
            Container.Bind<IHealingModificationDataSource>().To<HealingModificationDataSource>().AsSingle();
            Container.Bind<IDamageModificationDataSource>().To<DamageModificationDataSource>().AsSingle();
        }

        private void BindRepositories()
        {
            Container.Bind<IHitRecordRepository>().To<HitRecordRepository>().AsSingle();
            Container.Bind<IStatusRepository>().To<StatusRepository>().AsSingle();
            Container.Bind<IHealthRepository>().To<HealthRepository>().AsSingle();
            Container.Bind<IKillableRepository>().To<KillableRepository>().AsSingle();
            Container.Bind<IAligmentRepository>().To<AligmentRepository>().AsSingle();
            Container.Bind<ISkillAnimationRepository>().To<SkillAnimationRepository>().AsSingle();
            Container.Bind<IAttributesRepository>().To<AttributesRepository>().AsSingle();
            Container.Bind<IResourceRepository>().To<ResourceRepository>().AsSingle();
            Container.Bind<IHitboxRepository>().To<HitboxRepository>().AsSingle();
            Container.Bind<IHurtableRepository>().To<HurtableRepository>().AsSingle();
            Container.Bind<IPositionableRepository>().To<PositionableRepository>().AsSingle();
            Container.Bind<ISkillOwnerRepository>().To<SkillOwnerRepository>().AsSingle();
            Container.Bind<CharacterModelProvider>().FromNew().AsSingle();
            Container.Bind<IFrameDataRepository>().To<FrameDataRepository>().AsSingle();
            Container.Bind<IActorRepository>().To<ActorRepository>().AsSingle();
            Container.Bind<IStatusTimerRepository>().To<StatusTimerRepository>().AsSingle();
            Container.Bind<IHealingDamageInstanceRepository>().To<HealingDamageInstanceRepository>().AsSingle();

            Container.Bind<UnitApiProvider>().FromNew().AsSingle();
            Container.Bind<SkillApiProvider>().FromNew().AsSingle();
            Container.Bind<StatusApiProvider>().FromNew().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<ITickable>().To<ModelUpdateService>().AsSingle();
            Container.Bind<StatusLookup>().FromNew().AsSingle();
            Container.Bind<IStatusLookupService>().To<StatusLookup>().FromResolve();

            Container.Bind<HitHandler>().FromNew().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<StatusFactory>().FromNew().AsSingle();
            Container.Bind<DamageInstanceFactory>().FromNew().AsSingle();

            Container.Bind<UnitApiFactory>().FromNew().AsSingle();
            Container.Bind<SkillApiFactory>().FromNew().AsSingle();
            Container.Bind<StatusApiFactory>().FromNew().AsSingle();

            Container.Bind<ICharacterViewFactory>().To<CharacterViewFactory>().AsSingle();
        }

        private void BindUseCases()
        {
            Container.Bind<PrecacheAttributersUseCase>().FromNew().AsSingle();
            Container.Bind<UpdateActorsUseCase>().FromNew().AsSingle();
            Container.Bind<IActionStateChangeEventHandler>().To<ActionStateChangeHandler>().AsSingle();

            Container.Bind<CreateUnitUseCase>().FromNew().AsSingle();
            Container.Bind<ICreateUnitEventHandler>().To<UnitCreationHandler>().AsSingle();
            Container.Bind<ICreateUnitOutput>().To<ScenePresenter>().FromResolve();

            Container.Bind<CastSkillFromSlotUseCase>().FromNew().AsSingle();
            Container.Bind<ICastSkillEventHandler>().To<CastSkillEventHandler>().AsSingle();
            Container.Bind<ICastOutput>().To<UnitPresenter>().FromResolve();

            Container.Bind<MoveUseCase>().FromNew().AsSingle();
            Container.Bind<IMovementOutput>().To<UnitPresenter>().FromResolve();

            Container.Bind<GiveResourceUseCase>().FromNew().AsSingle();
            Container.Bind<IGiveResourceEventHandler>().To<GiveResourceEventHandler>().AsSingle();
            Container.Bind<IGiveResourceOutput>().To<UnitPresenter>().FromResolve();

            Container.Bind<SpendResourceUseCase>().FromNew().AsSingle();
            Container.Bind<ISpendResourceEventHandler>().To<SpendResourceEventHandler>().AsSingle();
            Container.Bind<ISpendResourceOutput>().To<UnitPresenter>().FromResolve();

            Container.Bind<GetHealthUseCase>().FromNew().AsSingle();

            Container.Bind<ApplyDamageUseCase>().FromNew().AsSingle();
            Container.Bind<IApplyDamageEventHandler>().To<ApplyDamageHandler>().AsSingle();

            Container.Bind<ApplyHealingUseCase>().FromNew().AsSingle();
            Container.Bind<IApplyHealingEventHandler>().To<ApplyHealingHandler>().AsSingle();

            Container.Bind<SetHealthUseCase>().FromNew().AsSingle();
            Container.Bind<IHealthOutput>().To<UnitPresenter>().FromResolve();

            Container.Bind<ApplyStatusUseCase>().FromNew().AsSingle();
            Container.Bind<IApplyStatusEventHandler>().To<ApplyStatusHandler>().AsSingle();

            Container.Bind<KillUnitUseCase>().FromNew().AsSingle();

            Container.Bind<GiveSkillUseCase>().FromNew().AsSingle();

            Container.Bind<FindStatusUseCase>().FromNew().AsSingle();

            Container.Bind<StartActionUseCase>().FromNew().AsSingle();

            //Hits
            Container.Bind<RecordHitUseCase>().FromNew().AsSingle();
            Container.Bind<HandleHitUseCase>().FromNew().AsSingle();
            Container.Bind<IHitEventHandler>().To<HitHandler>().FromResolve();

            //Statuses
            Container.Bind<StartStatusTimerUseCase>().FromNew().AsSingle();

            Container.Bind<StopStatusTimerUseCase>().FromNew().AsSingle();

            Container.Bind<UpdateStatusTimersUseCase>().FromNew().AsSingle();
            Container.Bind<IStatusTickEventHandler>().To<StatusTickHandler>().AsSingle();

            Container.Bind<RemoveStatusUseCase>().FromNew().AsSingle();
            Container.Bind<IRemoveStatusEventHandler>().To<RemoveStatusHandler>().AsSingle();

            Container.Bind<UpdateStatusesUseCases>().FromNew().AsSingle();
            Container.Bind<IStatusExpiredEventHandler>().To<StatusExpireHandler>().AsSingle();

            //Attributes
            Container.Bind<GetAttributeValueUseCase>().FromNew().AsSingle();
            Container.Bind<GetHasteModifierUseCase>().FromNew().AsSingle();
            Container.Bind<GetVersalityModifierUseCase>().FromNew().AsSingle();
        }
    }
}