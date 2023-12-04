using Assets.Sources.Temp;
using Core.Combat.Engine;
using Core.Combat.Utils;
using System;
using UnityEngine;

namespace Temp.Testing
{
    internal class CombatIniter : MonoBehaviour
    {
        private void Start()
        {
            Networking.Combat.UseClient(new AutoAcceptClient());
            CombatTime.SetStartTime(Environment.TickCount);
            Combat.Start();
        }

        private void OnDestroy()
        {
            Combat.Stop();
        }

        private void Update()
        {
            ModelUpdate.Update(UnityEngine.Time.deltaTime);
        }
    }
}
