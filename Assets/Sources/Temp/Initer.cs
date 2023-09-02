using Core.Combat.Engine;
using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Data;
using System;
using System.Threading;
using UnityEngine;
using View;
using View.Combat.UI.ActionBar;

namespace Temp.Testing
{
    internal class Initer : MonoBehaviour
    {
        [SerializeField] private int[] _spells;

        private Unit _unit;
        private Thread _update;

        private void Awake()
        {
            CombatTime.SetStartTime(Environment.TickCount);

            _update = new Thread(new ThreadStart(CombatInitializer.Initialize));
            _update.Priority = System.Threading.ThreadPriority.Highest;
            _update.Start();
        }

        //private void Start()
        //{
        //    List<Spell> spells = new List<Spell>();

        //    foreach (int id in _spells)
        //    {
        //        spells.Add(Core.Data.SpellLib.SpellLib.GetSpell(id));
        //    }

        //    _data.BasicSpells = spells.ToArray();

        //    _unit = UnitFactory.CreateUnit(_data, Team.Team.NONE, new Position() { Location = new Vector3(), ViewDirection = new Vector3() });

        //    _bar.SetActiveUnit(_unit);

        //    _unit.CastAbility(new EventData(_unit, _unit, _unit.GetAbility(Combat.Abilities.SpellSlot.FIRST).Spell));

        //    foreach (UnitView view in _setupUnits)
        //    {
        //        view.TryAssignTo(_unit);
        //    }
        //}

        private void Start()
        {

        }

        private void OnDestroy()
        {
            Combat.Stop();
            Combat.Reset();
        }
    }
}
