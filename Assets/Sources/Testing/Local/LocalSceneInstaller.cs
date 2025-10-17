using Combat.API;
using Combat.API.Controllers;
using Combat.API.Controllers.Factories;
using Combat.Common.ValueObjects;
using Combat.Local.Controllers;
using Combat.Local.Data.Databases;
using Combat.Local.Data.Factories;
using Combat.Local.Data.Lookup;
using Combat.Local.Data.Presentation;
using Combat.Local.Data.Repositories;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Scene;
using Combat.Local.Events;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Repositories;
using Combat.Local.Gateways.Repositories.Unit;
using Combat.Local.Presentation.Factories;
using Combat.Local.Presentation.Presenters;

using Temp.Domain.Implementations;

using Testing.Local.Temp.DomainOutputs;
using Testing.Local.Temp.Factories;

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
            BindApi();

            //Container.Bind<AssetProvider>().FromComponentsOn(gameObject).AsSingle();

            Container.Bind<ScenePresenter>().FromNew().AsSingle();
            Container.Bind<UnitPresenter>().FromNew().AsSingle();

            var val = Container.Resolve<CombatController>();
            Container.Resolve<SceneApiProvider>().Register(Container.Resolve<SceneApi>());
            Container.Resolve<SkillDataBase>();

            SetUpEventHandlers();

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
            Container.Bind<IActorRepository>().To<ActorRepository>().AsSingle();
            Container.Bind<IStatusTimerRepository>().To<StatusTimerRepository>().AsSingle();
            Container.Bind<IHealingDamageInstanceRepository>().To<HealingDamageInstanceRepository>().AsSingle();

            Container.Bind<ISkillRepository>().To<SkillRepository>().AsSingle();

        }

        private void BindServices()
        {
            Container.Bind<ITickable>().To<UpdateController>().AsSingle();
            Container.Bind<StatusLookup>().FromNew().AsSingle();
            Container.Bind<IStatusLookupService>().To<StatusLookup>().FromResolve();
        }

        private void BindFactories()
        {
            Container.Bind<StatusFactory>().FromNew().AsSingle();
            Container.Bind<DamageInstanceFactory>().FromNew().AsSingle();

            Container.Bind<ICharacterViewFactory>().To<CharacterViewFactory>().AsSingle();
            Container.Bind<IActionFactory>().To<ActionFactory>().AsSingle();
        }

        private void BindUseCases()
        {
            Container.Bind<PrecacheAttributersUseCase>().FromNew().AsSingle();
            Container.Bind<UpdateActorsUseCase>().FromNew().AsSingle();
            Container.Bind<IActionStateChangeEventHandler>().To<ActionStateChangeHandler>().AsSingle();

            Container.Bind<CreateCharacterUseCase>().FromNew().AsSingle();
            Container.Bind<CharacterCreatedHandler>().FromNew().AsSingle();
            Container.Bind<ICreateUnitEventHandler>().To<CharacterCreatedHandler>().FromResolve();
            Container.Bind<ICreateUnitOutput>().To<ScenePresenter>().FromResolve();

            Container.Bind<CastSkillFromSlotUseCase>().FromNew().AsSingle();
            Container.Bind<ISkillCastEventHandler>().To<SkillCastHandler>().AsSingle();
            Container.Bind<ICastOutput>().To<UnitPresenter>().FromResolve();

            Container.Bind<MoveUseCase>().FromNew().AsSingle();
            Container.Bind<IMovementOutput>().To<UnitPresenter>().FromResolve();

            Container.Bind<GiveResourceUseCase>().FromNew().AsSingle();
            Container.Bind<IGiveResourceEventHandler>().To<CharacterGiveResourceEventHandler>().AsSingle();
            Container.Bind<IGiveResourceOutput>().To<UnitPresenter>().FromResolve();

            Container.Bind<SpendResourceUseCase>().FromNew().AsSingle();
            Container.Bind<ISpendResourceEventHandler>().To<CharacterSpendResourceEventHandler>().AsSingle();
            Container.Bind<ISpendResourceOutput>().To<UnitPresenter>().FromResolve();

            Container.Bind<GetHealthUseCase>().FromNew().AsSingle();

            Container.Bind<ApplyDamageUseCase>().FromNew().AsSingle();
            Container.Bind<CharacterDamagedHandler>().FromNew().AsSingle();
            Container.Bind<IApplyDamageEventHandler>().To<CharacterDamagedHandler>().FromResolve();

            Container.Bind<ApplyHealingUseCase>().FromNew().AsSingle();
            Container.Bind<CharacterHealedHandler>().FromNew().AsSingle();
            Container.Bind<IApplyHealingEventHandler>().To<CharacterHealedHandler>().FromResolve();

            Container.Bind<SetHealthUseCase>().FromNew().AsSingle();
            Container.Bind<IHealthOutput>().To<UnitPresenter>().FromResolve();

            Container.Bind<ApplyStatusUseCase>().FromNew().AsSingle();
            Container.Bind<StatusCreateHandler>().FromNew().AsSingle();
            Container.Bind<IApplyStatusEventHandler>().To<StatusCreateHandler>().FromResolve();

            Container.Bind<KillUnitUseCase>().FromNew().AsSingle();

            Container.Bind<GiveSkillUseCase>().FromNew().AsSingle();

            Container.Bind<FindStatusUseCase>().FromNew().AsSingle();

            //Hits
            Container.Bind<RecordHitUseCase>().FromNew().AsSingle();
            Container.Bind<HandleHitUseCase>().FromNew().AsSingle();
            Container.Bind<HitHandler>().FromNew().AsSingle();
            Container.Bind<IHitEventHandler>().To<HitHandler>().FromResolve();

            //Statuses
            Container.Bind<StartStatusTimerUseCase>().FromNew().AsSingle();

            Container.Bind<StopStatusTimerUseCase>().FromNew().AsSingle();

            Container.Bind<UpdateStatusTimersUseCase>().FromNew().AsSingle();
            Container.Bind<StatusTickHandler>().FromNew().AsSingle();
            Container.Bind<IStatusTickEventHandler>().To<StatusTickHandler>().FromResolve();

            Container.Bind<RemoveStatusUseCase>().FromNew().AsSingle();
            Container.Bind<StatusRemoveHandler>().FromNew().AsSingle();
            Container.Bind<IRemoveStatusEventHandler>().To<StatusRemoveHandler>().FromResolve();

            Container.Bind<UpdateStatusesUseCases>().FromNew().AsSingle();
            Container.Bind<StatusExpireHandler>().FromNew().AsSingle();
            Container.Bind<IStatusExpiredEventHandler>().To<StatusExpireHandler>().FromResolve();

            //Attributes
            Container.Bind<GetAttributeValueUseCase>().FromNew().AsSingle();
            Container.Bind<GetHasteModifierUseCase>().FromNew().AsSingle();
            Container.Bind<GetVersalityModifierUseCase>().FromNew().AsSingle();
        }

        private void BindApi()
        {
            Container.Bind<UnitApiProvider>().FromNew().AsSingle();
            Container.Bind<SkillApiProvider>().FromNew().AsSingle();
            Container.Bind<StatusApiProvider>().FromNew().AsSingle();
            Container.Bind<SceneApiProvider>().FromNew().AsSingle();

            Container.Bind<IUnitApiFactory>().To<UnitApiFactory>().AsSingle();
            Container.Bind<ISkillApiFactory>().To<SkillApiFactory>().AsSingle();
            Container.Bind<IStatusApiFactory>().To<StatusApiFactory>().AsSingle();

            Container.Bind<SceneApi>().FromNew().AsSingle();

            Container.Bind<SceneEventApiController>().FromNew().AsSingle();
            Container.Bind<HitEventApiController>().FromNew().AsSingle();
            Container.Bind<CharacterEventApiController>().FromNew().AsSingle();
            Container.Bind<StatusEventApiController>().FromNew().AsSingle();
        }

        private void SetUpEventHandlers()
        {
            SceneEventApiController sceneEventApiController = Container.Resolve<SceneEventApiController>(); ;
            HitEventApiController hitEventApiController = Container.Resolve<HitEventApiController>();
            CharacterEventApiController characterEventApiController = Container.Resolve<CharacterEventApiController>();
            StatusEventApiController statusEventApiController = Container.Resolve<StatusEventApiController>();

            Container.Resolve<CharacterCreatedHandler>().Created += sceneEventApiController.HandleCharacterCreated;

            Container.Resolve<HitHandler>().Hitted += hitEventApiController.HandleHit;

            Container.Resolve<StatusCreateHandler>().Created += statusEventApiController.HandleCreate;
            Container.Resolve<StatusTickHandler>().Ticked += statusEventApiController.HandleTick;
            Container.Resolve<StatusExpireHandler>().Expired += statusEventApiController.HandleExpire;
            Container.Resolve<StatusRemoveHandler>().Removed += statusEventApiController.HandleRemove;

            Container.Resolve<CharacterDamagedHandler>().Damaged += characterEventApiController.HandleDamageRecived;
            Container.Resolve<CharacterDamagedHandler>().Damaged += characterEventApiController.HandleDamageDealth;

            Container.Resolve<CharacterHealedHandler>().Healed += characterEventApiController.HandleHealingRecived;
            Container.Resolve<CharacterHealedHandler>().Healed += characterEventApiController.HandleHealingDealth;


        }
    }
}