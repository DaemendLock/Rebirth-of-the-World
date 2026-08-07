using Combat.API.Adapters;
using Combat.API.Scripting;
using Combat.Local.Controllers;
using Combat.Local.Data.Databases;
using Combat.Local.Data.DataSources;
using Combat.Local.Data.Repositories;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.OutputPorts.Statuses;
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
using Combat.Local.Gateways.Repositories.Players;
using Combat.Local.Gateways.Repositories.Skills;
using Combat.Local.Presentation.Presenters;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Factories;
using Combat.Local.Scripting.Idk;
using Combat.Local.Scripting.Ports.Statuses;
using Combat.Local.Scripting.Runtime;
using Combat.Local.Scripting.SkillPorts;

using Global.Local.DTO;

using UnityEngine;

using Zenject;

namespace Combat.Local.Composition
{
    public sealed class CombatSceneInstaller : MonoInstaller
    {
        [SerializeField] private string _defaultLocationName = "test";

        public override void InstallBindings()
        {
            BindRepositories();
            BindFactories();
            BindUseCases();
            BindDataSources();
            BindScripting();
            BindControllers();
            BindPresenters();
            BindApi();
            BindStartup();

            ConfigureScripting();
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
            Container.Bind<ISkillMemoryRepository>().To<FixedSizeArraySkillMemoryRepository>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IAbilityFactory>().To<AbilityFactory>().AsSingle();
            Container.Bind<CustomScriptSkillStrategyFactory>().AsSingle();
            Container.Bind<NewScriptStrategyFactory>().AsSingle();

            Container.Bind<StatusFactory>().AsSingle();
            Container.Bind<CustomScriptStatusStrategyFactory>().AsSingle();

            Container.Bind<CharacterModelFactory>().AsSingle();
            Container.Bind<ActionFactory>().AsSingle();
            Container.Bind<IAbilityActionStrategyFactory>().To<AbilityActionStrategyFactory>().AsSingle();
        }

        private void BindUseCases()
        {
            Container.Bind<AttributeOwnerUpdateAllUseCase>().AsSingle();

            Container.Bind<CharacterCreateUseCase>().AsSingle();
            Container.Bind<DesireCastFromSlotUseCase>().AsSingle();
            Container.Bind<ReleaseSkillFromSlotUseCase>().AsSingle();
            Container.Bind<ResourceGiveUseCase>().AsSingle();
            Container.Bind<ResourceSpendUseCase>().AsSingle();
            Container.Bind<HealthApplyDamageUseCase>().AsSingle();
            Container.Bind<HealthApplyHealingUseCase>().AsSingle();
            Container.Bind<HealthSetUseCase>().AsSingle();

            Container.Bind<ActorForceKillUseCase>().AsSingle();
            Container.Bind<ActorReviveUseCase>().AsSingle();
            Container.Bind<ActorActAllUseCase>().AsSingle();
            Container.Bind<StatusOwnerFindStatusUseCase>().AsSingle();
            Container.Bind<AddMovementEffectUseCase>().AsSingle();

            Container.Bind<HitsHandleUseCase>().AsSingle();
            Container.Bind<AbilityProgressAllUseCase>().AsSingle();

            Container.Bind<StatusOwnerApplyUseCase>().AsSingle();
            Container.Bind<StatusTimerStartUseCase>().AsSingle();
            Container.Bind<StatusTimerStopUseCase>().AsSingle();
            Container.Bind<StatusRemoveUseCase>().AsSingle();
            Container.Bind<StatusOwnerProgressAllUseCases>().AsSingle();

            Container.Bind<AttributeOwnerGetAttributeValueUseCase>().AsSingle();
            Container.Bind<AttributeOwnerGetHasteModifierUseCase>().AsSingle();
            Container.Bind<AttributeOwnerGetVersalityModifierUseCase>().AsSingle();

            Container.Bind<FindCharactersInRadiusUseCase>().AsSingle();

            Container.Bind<AssumeControlOverCharacterUseCase>().AsSingle();
            Container.Bind<RotateUseCase>().AsSingle();
            Container.Bind<DesireMoveInDirectionUseCase>().AsSingle();
            Container.Bind<CreatePlayerUseCase>().AsSingle();
        }

        private void BindDataSources()
        {
            Container.Bind<SkillDataBase>().AsSingle();
            Container.Bind<ISkillDataBase>().To<SkillDataBase>().FromResolve();
            Container.Bind<IActionDataContainer>().To<SkillDataBase>().FromResolve();
            Container.Bind<ISkillScriptTypeProvider>().To<SkillDataBase>().FromResolve();

            StatusScriptTypeDataSource statusDataSource = new(typeof(StatusScript));
            Container.Bind<IStatusDataBase>().FromInstance(statusDataSource);
            Container.Bind<IStatusScriptTypeProvider>().FromInstance(statusDataSource);

            Container.Bind<SceneObjectDataSource>().AsSingle();
            Container.Bind<ISceneObjectDataSource>().To<SceneObjectDataSource>().FromResolve();
            Container.Bind<ICharacterPrefabDataSource>().To<CharacterPrefabDataSource>().AsSingle();
        }

        private void BindScripting()
        {
            Container.Bind<ScriptSkillRuntimeRegistry>().AsSingle();
            Container.Bind<ISkillRuntimeRegistry>().To<ScriptSkillRuntimeRegistry>().FromResolve();
            Container.Bind<ScriptStatusRuntimeRegistry>().AsSingle();
            Container.Bind<IStatusRuntimeRegistry>().To<ScriptStatusRuntimeRegistry>().FromResolve();

            Container.Bind<PropertySkillLyfecycleHandler>().AsSingle();
            Container.Bind<ISkillLyfecycleHandler>().To<PropertySkillLyfecycleHandler>().FromResolve();
            Container.Bind<ISkillHitHandler>().To<PropertySkillHitHandler>().AsSingle();
            Container.Bind<ISkillActionStateChangeHandler>().To<PropertySkillActionStateChangeHandler>().AsSingle();
            Container.Bind<ISkillExecutionPort>().To<SkillExecutionPort>().AsSingle();

            Container.Bind<PropertyStatusLifecycleHandler>().AsSingle();
            Container.Bind<IStatusLifecycleHandler>().To<PropertyStatusLifecycleHandler>().FromResolve();
            Container.Bind<IStatusTickHandler>().To<StatusPropertyTickHandler>().AsSingle();
            Container.Bind<IStatusAttributeCalculator>().To<StatusAttributeModifierCalculator>().AsSingle();
            Container.Bind<IHealingDamageModifierCalculator>().To<PropertyDamageModifcationCalculator>().AsSingle();
            Container.Bind<IDamageResultHandler>().To<StatusPropertyDamageResultHandler>().AsSingle();

            Container.Bind<UnitNewAdapter>().AsSingle();
        }

        private void BindControllers()
        {
            Container.Bind<EncounterController>().AsSingle();
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle();
        }

        private void BindPresenters()
        {
            Container.Bind<ScenePresenter>().AsSingle();
            Container.Bind<CharacterPresenter>().AsSingle();
            Container.Bind<PlayerPresenter>().AsSingle();

            Container.Bind<ICharacterCreateOutput>().To<ScenePresenter>().FromResolve();
            Container.Bind<IGiveResourceOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<ISpendResourceOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<IHealthOutput>().To<CharacterPresenter>().FromResolve();
            Container.Bind<ICharacterConsciousStateOutput>().To<CharacterPresenter>().FromResolve();

            Container.Bind<ITakeControllOutput>().To<PlayerPresenter>().FromResolve();
            Container.Bind<IDesireCastOutput>().To<PlayerPresenter>().FromResolve();
        }

        private void BindApi()
        {
            Container.Bind<ISceneApiAdapter>().To<SceneApiAdapter>().AsSingle();
            Container.Bind<CharacterApiAdapter>().AsSingle();
            Container.Bind<AbilityApiAdapter>().AsSingle();
            Container.Bind<IStatusApiAdapter>().To<StatusApiAdapter>().AsSingle();

            Container.Bind<StatusFacade>().AsSingle();
            Container.Bind<HealthOwnerFacade>().AsSingle();
            Container.Bind<AttributeOwnerFacade>().AsSingle();
            Container.Bind<CharacterFacade>().AsSingle();
            Container.Bind<AbilityFacade>().AsSingle();
            Container.Bind<EncounterFacade>().AsSingle();
        }

        private void BindStartup()
        {
            if (Container.HasBinding<StartCombatRequest>() == false)
            {
                string locationName = string.IsNullOrWhiteSpace(_defaultLocationName)
                    ? "test"
                    : _defaultLocationName;

                Container.BindInstance(new StartCombatRequest(locationName)).AsSingle();
            }

            Container.Bind<ILocationSceneLoader>().To<ZenjectLocationSceneLoader>().AsSingle();
            Container.BindInterfacesTo<CombatBootstrap>().AsSingle();
        }

        private void ConfigureScripting()
        {
            PropertySkillLyfecycleHandler skillLifecycleHandler = Container.Resolve<PropertySkillLyfecycleHandler>();
            skillLifecycleHandler.RegisterStrategyFactory(Container.Resolve<CustomScriptSkillStrategyFactory>());
            skillLifecycleHandler.RegisterStrategyFactory(Container.Resolve<NewScriptStrategyFactory>());

            PropertyStatusLifecycleHandler statusLifecycleHandler = Container.Resolve<PropertyStatusLifecycleHandler>();
            statusLifecycleHandler.RegisterStrategyFactory(Container.Resolve<CustomScriptStatusStrategyFactory>());
        }
    }
}
