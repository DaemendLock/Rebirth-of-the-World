using Adapters.Combat;
using Core.Combat.Team;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Temp.Template;
using UnityEngine;
using Utils.DataStructure;
using Utils.DataTypes;
using View.Combat.Units;

namespace Assets.Sources.Temp.Template
{
    internal class EncounterTemplate : MonoBehaviour
    {
        [SerializeField] private int _maxPlayres;
        [SerializeField] private CharacterTemplate[] _characters;

        public void LoadChractersToCombat()
        {

            for (int i = 0; i < _characters.Length; i++)
            {
                UnitCreationData.ModelData mdata = new UnitCreationData.ModelData(Array.ConvertAll(_characters[i]._spellIds, v => (SpellId) v), StatsTable.UNIT_DEFAULT, new UnitCreationData.PositionData(), new UnitCreationData.CastResourceData(), (byte) (i%2), new int[0]);

                UnitCreationData.ViewData vdata = new UnitCreationData.ViewData();

                UnitCreationData udata = new UnitCreationData(i, mdata, vdata);

                Core.Combat.Engine.Combat.CreateUnit(udata);
                UnitFactory.CreateUnit(udata);
                SelectionInfo.RegisterControllUnit(udata.Id, udata.ControlGroup);
            }
        }
    }
}
