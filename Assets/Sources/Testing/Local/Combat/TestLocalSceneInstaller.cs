using Assets.Sources.Testing.Local;

using Client.Testing.View;

using Combat.Local.Scripting.Ports;

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

            Container.Resolve<ObjectiveDispatcher>().Create(new(new(), "kill"));
        }
    }
}
