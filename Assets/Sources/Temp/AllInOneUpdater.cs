using System.Threading.Tasks;

using UnityEngine;

using Zenject;

namespace Assets.Sources.Temp
{

    public class AllInOneUpdater : MonoBehaviour
    {
        //[Inject] private Client.Combat.Infrastructure.Controllers.ICombatController _clientCombatController;
        //[Inject] private Server.Combat.Infrastructure.Controllers.ICombatController _serverCombatController;
        //[Inject] private ClientInputReader _clientInputReader;

        public void Update()
        {
            //_clientCombatController?.Tick();
            //Task.Run(_serverCombatController.Tick);
            //_clientInputReader?.Tick();
        }
    }
}
