using Assets.Sources.Testing.Local.Lobby.Temp;

using Lobby.Local.Data.DataSources;
using Lobby.Local.Data.Repositories;
using Lobby.Local.Domain.Outputs;
using Lobby.Local.Domain.Repositories;
using Lobby.Local.Domain.UseCases.Accounts;
using Lobby.Local.Domain.UseCases.CharacterGallery;
using Lobby.Local.Domain.UseCases.Scenarios;
using Lobby.Local.Presentation.Controllers;
using Lobby.Local.Presentation.Misc;
using Lobby.Local.Presentation.Presenters;

using Zenject;

namespace Testing.Local.Lobby
{

    public sealed class LobbySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindRepositories();

            BindUseCases();
            BindPresenters();

            Container.Bind<ICharacterListDataSource>().To<LocalCharacterDatabase>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IAssetProvider>().To<LazyAssetProvider>().FromComponentInHierarchy().AsSingle();

            Container.Bind<UiNavigationService>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterGalleryController>().FromNew().AsSingle();
        }

        private void BindPresenters()
        {
            Container.Bind<ScenarioWindowPresenter>().FromNew().AsSingle();
            Container.Bind<IScenarioCreateOutput>().To<ScenarioWindowPresenter>().FromResolve();
            Container.Bind<IScenarioCancelOutput>().To<ScenarioWindowPresenter>().FromResolve();
            Container.Bind<IScenarioStartOutput>().To<ScenarioStartSceneOutput>().AsSingle();

            Container.Bind<AccountPresenter>().FromNew().AsSingle();
            Container.Bind<IAccountCreateOutput>().To<AccountPresenter>().FromResolve();
        }

        private void BindRepositories()
        {
            Container.Bind<IScenarioRepository>().To<LocalScenarioRepository>().AsSingle();

            Container.Bind<IAccountRepository>().To<AccountRepository>().AsSingle();

            Container.Bind<ICharacterRepository>().To<CharacterRepository>().AsSingle();
        }

        private void BindUseCases()
        {
            //Scenarios
            Container.Bind<ScenarioCreateUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioCancelUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioJoinUseCase>().FromNew().AsSingle();
            Container.Bind<ScenarioStartUseCase>().FromNew().AsSingle();

            //Accounts
            Container.Bind<AccountGetAvailableScenariosUseCase>().FromNew().AsSingle();
            Container.Bind<AccountCreateUseCase>().FromNew().AsSingle();
            Container.Bind<FindCharactersUseCase>().FromNew().AsSingle();
        }
    }
}
