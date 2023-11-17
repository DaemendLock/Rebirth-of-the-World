using Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Temp
{
    internal class LobbyIniter : MonoBehaviour
    {
        private void Start()
        {
            Networking.Networking.Connect("localhost", 27122);
            
        }
    }
}
