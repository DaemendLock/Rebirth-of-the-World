using Assets.Sources.Testing.Local;

using Client.Testing.View;

using Combat.API.Adapters;
using Combat.API.API.Skills;
using Combat.API.Scripting;
using Combat.Local.Controllers;
using Combat.Local.Data.Databases;
using Combat.Local.Data.DataSources;
using Combat.Local.Data.Factories;
using Combat.Local.Data.Repositories;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Repositories.Skill;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Character;
using Combat.Local.Domain.UseCases.Players;
using Combat.Local.Domain.UseCases.Scene;
using Combat.Local.Domain.UseCases.Skills;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Factories;
using Combat.Local.Gateways.Repositories;
using Combat.Local.Gateways.Repositories.Characters;
using Combat.Local.Gateways.Repositories.Encounter;
using Combat.Local.Gateways.Repositories.Players;
using Combat.Local.Gateways.Repositories.Skills;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Presenters;

using Local.Combat.LazyData;

using Zenject;

namespace Testing.Local.Combat
{
    public sealed class LocalSceneInstaller : MonoInstaller
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
            Container.Bind<PlayerModelComponent>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LocalInputReader>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ITestMenuStrategy>().To<TestMenuStrategy>().AsSingle();

            IAbilityFactory skillFactory = Container.Resolve<IAbilityFactory>();
            skillFactory.RegisterStrategyFactory(Container.Resolve<CustomScriptSkillStrategyFactory>());
            skillFactory.RegisterStrategyFactory(Container.Resolve<NewScriptStrategyFactory>());

            StatusFactory statusFactory = Container.Resolve<StatusFactory>();
            statusFactory.RegisterStrategyFactory(Container.Resolve<CustomScriptStatusStrategyFactory>());

            //StatsTable stats = StatsTable.UnitDefault;
            //stats[Attribute.Speed] = new(1, 100);
            //System.Span<SkillId> skills = stackalloc SkillId[] { new(1001) };
            //var id = combatController.CreateUnit(new(new("Katerina"), new(2), new(5, 5, 5), 1000, 1000, stats.ToAttributeArray(), System.Array.Empty<ResourceValue>(), skills));
        }

        private void BindControllers()
        {
            Container.Bind<EncounterController>().FromNew().AsSingle();
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ITickable>().To<UpdateController>().AsSingle();
        }

        private void BindPresenters()
        {
            Container.Bind<ScenePresenter>().FromNew().AsSingle();
            Container.Bind<CharacterPresenter>().FromNew().AsSingle();
            Container.Bind<PlayerPresenter>().FromNew().AsSingle();

            Container.Bind<ICharacterCreateOutput>().To<ScenePresenter>().FromResolve();
            Container.Bind<IGiveResourceOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<ISpendResourceOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<IHealthOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<ICharacterConsciousStateOutput>().To<CharacterPresenter>().FromResolve();

            Container.Bind<ITakeControllOutput>().To<PlayerPresenter>().FromResolve();
            Container.Bind<IDesireCastOutput>().To<PlayerPresenter>().FromResolve();
        }

        private void BindDataSources()
        {
            Container.Bind<SkillDataBase>().FromNew().AsSingle();
            Container.Bind<ISkillDataBase>().To<SkillDataBase>().FromResolve();
            Container.Bind<IActionDataContainer>().To<SkillDataBase>().FromResolve();

            Container.Bind<IStatusDataBase>().FromInstance(new StatusScriptTypeDataSource(typeof(StatusScript))).AsSingle();

            Container.Bind<SceneObjectDataSource>().FromNew().AsSingle();
            Container.Bind<ISceneObjectDataSource>().To<SceneObjectDataSource>().FromResolve();

            Container.Bind<ILocationDataSource>().To<LazyEnviromentDataSource>().FromComponentInHierarchy().AsSingle();
        }

        private void BindRepositories()
        {
            Container.Bind<IStatusRepository>().To<StatusRepository>().AsSingle();
            Container.Bind<IHealthRepository>().To<DictionaryHealthRepository>().AsSingle();
            Container.Bind<IAligmentRepository>().To<AligmentRepository>().AsSingle();
            Container.Bind<IAttributesRepository>().To<AttributesRepository>().AsSingle();
            Container.Bind<IResourceOwnerRepository>().To<ResourceRepository>().AsSingle();
            Container.Bind<IHitboxOwnerRepository>().To<HitboxRepository>().AsSingle();
            Container.Bind<IHurtableRepository>().To<HurtableRepository>().AsSingle();
            Container.Bind<IPositionableRepository>().To<PositionableRepository>().AsSingle();
            Container.Bind<ISkillOwnerRepository>().To<SkillOwnerRepository>().AsSingle();
            Container.Bind<IActorRepository>().To<ActorRepository>().AsSingle();
            Container.Bind<IStatusTimerRepository>().To<StatusTimerRepository>().AsSingle();
            Container.Bind<ICharacterUpdateRepository>().To<UpdateTargetRepository>().AsSingle();
            Container.Bind<IAbilityRepository>().To<AbilityRepository>().AsSingle();
            Container.Bind<IStatusOwnerRepository>().To<StatusOwnerRepository>().AsSingle();
            Container.Bind<IMovementEffectOwnerRepository>().To<MovementEffectOwnerRepository>().AsSingle();
            Container.Bind<IPlayerRepository>().To<PlayerRepository>().AsSingle();

            Container.Bind<ICharacterPrefabDataSource>().To<CharacterPrefabDataSource>().AsSingle();

            //Container.Bind<IEncounterProvider>().To<LocalEncounterRepository>().AsSingle();

            Container.Bind<ISkillMemoryRepository>().To<FixedSizeArraySkillMemoryRepository>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IAbilityFactory>().To<AbilityFactory>().AsSingle();
            Container.Bind<CustomScriptSkillStrategyFactory>().FromNew().AsSingle();
            Container.Bind<NewScriptStrategyFactory>().FromNew().AsSingle();

            Container.Bind<StatusFactory>().FromNew().AsSingle();
            Container.Bind<CustomScriptStatusStrategyFactory>().FromNew().AsSingle();

            Container.Bind<CharacterModelFactory>().FromNew().AsSingle();
            Container.Bind<ActionFactory>().FromNew().AsSingle();
            Container.Bind<IActionStrategyFactory>().To<ActionStrategyFactory>().AsSingle();
        }

        private void BindUseCases()
        {
            Container.Bind<AttributeOwnerUpdateAllUseCase>().FromNew().AsSingle();

            Container.Bind<CharacterCreateUseCase>().FromNew().AsSingle();
            Container.Bind<DesireCastFromSlotUseCase>().FromNew().AsSingle();
            Container.Bind<ResourceGiveUseCase>().FromNew().AsSingle();
            Container.Bind<ResourceSpendUseCase>().FromNew().AsSingle();
            Container.Bind<HealthApplyDamageUseCase>().FromNew().AsSingle();
            Container.Bind<HealthApplyHealingUseCase>().FromNew().AsSingle();

            Container.Bind<HealthSetUseCase>().FromNew().AsSingle();

            Container.Bind<ActorForceKillUseCase>().FromNew().AsSingle();
            Container.Bind<ActorReviveUseCase>().FromNew().AsSingle();
            Container.Bind<ActorActAllUseCase>().FromNew().AsSingle();
            Container.Bind<StatusOwnerFindStatusUseCase>().FromNew().AsSingle();

            Container.Bind<AddMovementEffectUseCase>().FromNew().AsSingle();

            //Hits
            Container.Bind<HitsHandleUseCase>().FromNew().AsSingle();

            //Abilities
            Container.Bind<AbilityProgressAllUseCase>().FromNew().AsSingle();

            //Statuses
            Container.Bind<StatusApplyUseCase>().FromNew().AsSingle();
            Container.Bind<StatusTimerStartUseCase>().FromNew().AsSingle();
            Container.Bind<StatusTimerStopUseCase>().FromNew().AsSingle();
            Container.Bind<StatusRemoveUseCase>().FromNew().AsSingle();
            Container.Bind<StatusOwnerProgressAllUseCases>().FromNew().AsSingle();

            //Attributes
            Container.Bind<AttributeOwnerGetAttributeValueUseCase>().FromNew().AsSingle();
            Container.Bind<AttributeOwnerGetHasteModifierUseCase>().FromNew().AsSingle();
            Container.Bind<AttributeOwnerGetVersalityModifierUseCase>().FromNew().AsSingle();

            //Scene
            Container.Bind<FindCharactersInRadiusUseCase>().FromNew().AsSingle();

            //Player
            Container.Bind<AssumeControlOverCharacterUseCase>().FromNew().AsSingle();
            Container.Bind<RotateUseCase>().FromNew().AsSingle();
            Container.Bind<DesireMoveInDirectionUseCase>().FromNew().AsSingle();
            Container.Bind<CreatePlayerUseCase>().FromNew().AsSingle();
        }

        private void BindApi()
        {
            Container.Bind<ISceneApiAdapter>().To<SceneApiAdapter>().AsSingle();
            Container.Bind<ICharacterApiAdapter>().To<CharacterApiAdapter>().AsSingle();
            Container.Bind<IAbilityApiAdapter>().To<AbilityApiAdapter>().AsSingle();
            Container.Bind<IStatusApiAdapter>().To<StatusApiAdapter>().AsSingle();

            Container.Bind<StatusFacade>().FromNew().AsSingle();
            Container.Bind<HealthOwnerFacade>().FromNew().AsSingle();
            Container.Bind<AttributeOwnerFacade>().FromNew().AsSingle();
            Container.Bind<CharacterFacade>().FromNew().AsSingle();
            Container.Bind<AbilityFacade>().FromNew().AsSingle();
            Container.Bind<EncounterFacade>().FromNew().AsSingle();
        }
    }
}