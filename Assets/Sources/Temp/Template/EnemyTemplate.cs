using Core.Combat.Units.Components;
using Core.Lobby.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Sources.Temp.Template
{
    [Serializable]
    internal class EnemyTemplate
    {
        [SerializeField] private int CharacterId;
        [SerializeField] private UnitState State;
        
    }
}
