using Assets.Sources.Testing.Local;

using Client.Testing.View;

using Combat.API.Adapters;
using Combat.Common.ValueObjects;
using Combat.Local.Controllers;
using Combat.Local.Data.Databases;
using Combat.Local.Data.DataSources;
using Combat.Local.Data.Factories;
using Combat.Local.Data.Repositories;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Repositories.Skills;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Scene;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Repositories;
using Combat.Local.Gateways.Repositories.Characters;
using Combat.Local.Gateways.Repositories.Unit;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Presenters;

using Temp.Domain.Implementations;

using Testing.Local.Temp.Factories;

using Zenject;

namespace Testing.Local
{
    public partial class LocalSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindRepositories();
            BindFactories();
            BindUseCases();
            BindDataSources();
            BindControllers();
            BindPresenters();
            BindApi();

            //Container.Bind<AssetProvider>().FromComponentsOn(gameObject).AsSingle();
            Container.Bind<CameraView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LocalInputReader>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ITestMenuStrategy>().To<TestMenuStrategy>().AsSingle();

            var combatController = Container.Resolve<CombatController>();
            Container.Resolve<SkillDataBase>();

            ISkillFactory skillFactory = Container.Resolve<ISkillFactory>();
            skillFactory.RegisterStrategyFactory(Container.Resolve<CustomScriptSkillStrategyFactory>());

            StatusFactory statusFactory = Container.Resolve<StatusFactory>();
            statusFactory.RegisterStrategyFactory(Container.Resolve<CustomScriptStatusStrategyFactory>());

            StatsTable stats = StatsTable.UnitDefault;
            stats[Attribute.Speed] = new(1, 100);
            System.Span<SkillId> skills = stackalloc SkillId[] { new(1001) };
            var id = combatController.CreateUnit(new(new("Katerina"), new(2), new(5, 5, 5), 1000, 1000, stats.ToAttributeArray(), skills));
        }

        private void BindControllers()
        {
            Container.Bind<CombatController>().FromNew().AsSingle();
            Container.Bind<PlayerController>().FromNew().AsSingle();
            Container.Bind<HitController>().FromNew().AsSingle();
            Container.Bind<ITickable>().To<UpdateController>().AsSingle();
        }

        private void BindPresenters()
        {
            Container.Bind<ScenePresenter>().FromNew().AsSingle();
            Container.Bind<CharacterPresenter>().FromNew().AsSingle();

            Container.Bind<ICreateUnitOutput>().To<ScenePresenter>().FromResolve();
            Container.Bind<IActionOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<IMovementOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<IGiveResourceOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<ISpendResourceOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<IHealthOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<ICharacterConsciousStateOutput>().To<CharacterPresenter>().FromResolve();

            Container.Bind<ITakeControllOutput>().To<PlayerPresenter>().AsSingle();
        }

        private void BindDataSources()
        {
            Container.Bind<SkillDataBase>().FromNew().AsSingle();
            Container.Bind<ISkillDataBase>().To<SkillDataBase>().FromResolve();
            Container.Bind<IActionAnimationProvider>().To<SkillDataBase>().FromResolve();

            Container.Bind<SceneCharacterModelDataSource>().FromNew().AsSingle();
            Container.Bind<ICharacterViewContainer>().To<SceneCharacterModelDataSource>().FromResolve();
            Container.Bind<ICharacterModelDataSource>().To<SceneCharacterModelDataSource>().FromResolve();
        }

        private void BindRepositories()
        {
            Container.Bind<IHitRecordRepository>().To<HitRecordRepository>().AsSingle();
            Container.Bind<IStatusRepository>().To<StatusRepository>().AsSingle();
            Container.Bind<IHealthRepository>().To<HealthRepository>().AsSingle();
            Container.Bind<IStateRepository>().To<StateRepository>().AsSingle();
            Container.Bind<IAligmentRepository>().To<AligmentRepository>().AsSingle();
            Container.Bind<IAttributesRepository>().To<AttributesRepository>().AsSingle();
            Container.Bind<IResourceRepository>().To<ResourceRepository>().AsSingle();
            Container.Bind<IHitboxRepository>().To<HitboxRepository>().AsSingle();
            Container.Bind<IHurtableRepository>().To<HurtableRepository>().AsSingle();
            Container.Bind<IPositionableRepository>().To<PositionableRepository>().AsSingle();
            Container.Bind<ISkillOwnerRepository>().To<SkillOwnerRepository>().AsSingle();
            Container.Bind<CharacterModelProvider>().FromNew().AsSingle();
            Container.Bind<IActorRepository>().To<ActorRepository>().AsSingle();
            Container.Bind<IStatusTimerRepository>().To<StatusTimerRepository>().AsSingle();
            Container.Bind<ICharacterUpdateList>().To<UpdateTargetList>().AsSingle();
            Container.Bind<ISkillRepository>().To<SkillRepository>().AsSingle();
            Container.Bind<IStatusOwnerRepository>().To<StatusOwnerRepository>().AsSingle();
            Container.Bind<IMovementEffectRepository>().To<MovementEffectRepository>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<ISkillFactory>().To<SkillFactory>().AsSingle();
            Container.Bind<CustomScriptSkillStrategyFactory>().FromNew().AsSingle();

            Container.Bind<StatusFactory>().FromNew().AsSingle();
            Container.Bind<CustomScriptStatusStrategyFactory>().FromNew().AsSingle();

            Container.Bind<CharacterModelFactory>().FromNew().AsSingle();
            Container.Bind<ActionFactory>().FromNew().AsSingle();
            Container.Bind<IActionStrategyFactory>().To<ActionStrategyFactory>().AsSingle();
            Container.Bind<MoveInDirectionEffectFactory>().To<MoveInDirectionEffectFactory>().AsSingle();
        }

        private void BindUseCases()
        {
            Container.Bind<UpdateAttributersUseCase>().FromNew().AsSingle();
            Container.Bind<UpdateActorsUseCase>().FromNew().AsSingle();

            Container.Bind<CreateCharacterUseCase>().FromNew().AsSingle();
            Container.Bind<CastSkillFromSlotUseCase>().FromNew().AsSingle();
            Container.Bind<MoveInDirectionUseCase>().FromNew().AsSingle();
            Container.Bind<GiveResourceUseCase>().FromNew().AsSingle();
            Container.Bind<SpendResourceUseCase>().FromNew().AsSingle();
            Container.Bind<ApplyDamageUseCase>().FromNew().AsSingle();
            Container.Bind<ApplyHealingUseCase>().FromNew().AsSingle();

            Container.Bind<GetHealthUseCase>().FromNew().AsSingle();
            Container.Bind<SetHealthUseCase>().FromNew().AsSingle();

            Container.Bind<ForceKillUseCase>().FromNew().AsSingle();
            Container.Bind<ReviveUseCase>().FromNew().AsSingle();
            Container.Bind<FindStatusUseCase>().FromNew().AsSingle();

            Container.Bind<AddMovementEffectUseCase>().FromNew().AsSingle();

            //Hits
            Container.Bind<RecordHitUseCase>().FromNew().AsSingle();
            Container.Bind<HandleHitsUseCase>().FromNew().AsSingle();

            //Statuses
            Container.Bind<ApplyStatusUseCase>().FromNew().AsSingle();
            Container.Bind<StartStatusTimerUseCase>().FromNew().AsSingle();
            Container.Bind<StopStatusTimerUseCase>().FromNew().AsSingle();
            Container.Bind<UpdateStatusTimersUseCase>().FromNew().AsSingle();
            Container.Bind<RemoveStatusUseCase>().FromNew().AsSingle();
            Container.Bind<UpdateStatusesUseCases>().FromNew().AsSingle();

            //Attributes
            Container.Bind<GetAttributeValueUseCase>().FromNew().AsSingle();
            Container.Bind<GetHasteModifierUseCase>().FromNew().AsSingle();
            Container.Bind<GetVersalityModifierUseCase>().FromNew().AsSingle();

            //Scene
            Container.Bind<FindCharactersInRadiusUseCase>().FromNew().AsSingle();

            //Player
            Container.Bind<AssumeControllOverCharacterUseCase>().FromNew().AsSingle();
        }

        private void BindApi()
        {
            Container.Bind<SceneApiAdapter>().FromNew().AsSingle();
            Container.Bind<CharacterApiAdapter>().FromNew().AsSingle();
            Container.Bind<SkillApiAdapter>().FromNew().AsSingle();
            Container.Bind<StatusApiAdapter>().FromNew().AsSingle();

            Container.Bind<StatusFacade>().FromNew().AsSingle();
            Container.Bind<HealthOwnerFacade>().FromNew().AsSingle();
            Container.Bind<AttributeOwnerFacade>().FromNew().AsSingle();
            Container.Bind<CharacterFacade>().FromNew().AsSingle();
            Container.Bind<SkillFacade>().FromNew().AsSingle();
            Container.Bind<SceneFacade>().FromNew().AsSingle();
        }
    }
}