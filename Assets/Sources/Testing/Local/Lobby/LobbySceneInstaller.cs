using Assets.Sources.Testing.Local.Lobby.Temp;

using Lobby.Local.Data.DataSources;
using Lobby.Local.Data.Repositories;
using Lobby.Local.Domain.Repositories;
using Lobby.Local.Domain.UseCases.Accounts;
using Lobby.Local.Domain.UseCases.CharacterGallery;
using Lobby.Local.Domain.UseCases.General;
using Lobby.Local.Domain.UseCases.Scenarios;
using Lobby.Local.Presentation.Controllers;
using Lobby.Local.Presentation.Misc;
using Lobby.Local.Presentation.Presenters;
using Lobby.Local.Presentation.View;

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

            Container.Bind<LobbyController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<CharacterGalleryController>().FromNew().AsSingle();
        }

        private void BindPresenters()
        {
            Container.Bind<ScenarioPresenter>().FromNew().AsSingle();
            Container.Bind<IScenarioCreateOutput>().To<ScenarioPresenter>().FromResolve();
            Container.Bind<IScenarioCancelOutput>().To<ScenarioPresenter>().FromResolve();

            Container.Bind<AccountPresenter>().FromNew().AsSingle();
            Container.Bind<IAccountCreateOutput>().To<AccountPresenter>().FromResolve();
        }

        private void BindRepositories()
        {
            Container.Bind<IScenarioRepository>().To<LocalScenarioRepository>().AsSingle();

            Container.Bind<IAccountRepository>().To<AccountRepository>().AsSingle();

            Container.Bind<ICharacterRepository>().To<CharacterRepository>().AsSingle();

            Container.Bind<ILobbyStateContainer>().To<LobbyStateContainer>().AsSingle();
        }

        private void BindUseCases()
        {
            //Lobby
            Container.Bind<LobbyOpenTabUseCase>().FromNew().AsSingle();
            Container.Bind<LobbyGoBackUseCase>().FromNew().AsSingle();
            Container.Bind<LobbyGoHomeUseCase>().FromNew().AsSingle();

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