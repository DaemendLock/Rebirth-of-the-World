using Core.Combat.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Temp.Testing
{
    internal class Initer : MonoBehaviour
    {
        private void Start()
        {
            CombatInitializer.Initialize();
        }
    }
}
