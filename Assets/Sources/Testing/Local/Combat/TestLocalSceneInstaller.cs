using Assets.Sources.Testing.Local;

using Client.Testing.View;

using Combat.API.Objectives;
using Combat.Local.Domain.UseCases.Objectives;
using Combat.Local.Scripting.Factories;

using Zenject;

namespace Testing.Local.Combat
{
    public sealed class TestLocalSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ITickable>()
                .To<UpdateController>()
                .AsSingle();

            Container.Bind<LocalInputReader>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container.Bind<ITestMenuStrategy>()
                .To<TestMenuStrategy>()
                .AsSingle();

            ObjectiveScriptFactory objectiveScriptFactory = Container.Resolve<ObjectiveScriptFactory>();
            objectiveScriptFactory.Register<DealDamageObjective>();
            objectiveScriptFactory.Register<BasicKillUnitObjective>();

            Container.Resolve<ObjectiveCreateUseCase>().Execute("kill");
        }
    }
}
