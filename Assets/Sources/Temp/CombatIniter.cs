using Assets.Sources.Temp;

using Core.Combat.Engine;

using Server.Combat;

using UnityEngine;

namespace Temp.Testing
{
    internal class CombatIniter : MonoBehaviour
    {
        private void Start()
        {
            Networking.Combat.UseClient(new AutoAcceptClient());
            Combat.Start();
        }

        private void OnDestroy() => Combat.Stop();

        private void Update() => CombatServer.Update(Time.deltaTime);
    }
}
