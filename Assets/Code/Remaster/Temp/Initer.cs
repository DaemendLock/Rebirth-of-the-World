using Core.Combat.Abilities;
using Core.Combat.Units;
using Core.Combat.Utils;
using Core.CombatSetup;
using Core.Data;
using Core.Engine;
using Core.View;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using View;

namespace Core.Temp
{
    public class Initer : MonoBehaviour
    {
        [SerializeField] private ActionBar _bar;
        [SerializeField] private UnitData _data;
        [SerializeField] private UnitView[] _setupUnits;

        [SerializeField] private int[] _spells;

        private Unit _unit;
        private Thread _update;

        private void Awake()
        {
            CombatTime.SetStartTime(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            _update = new Thread(new ThreadStart(Engine.Combat.Start));
            _update.Priority = System.Threading.ThreadPriority.Highest;
            _update.Start();
        }

        private void Start()
        {
            List<Spell> spells = new List<Spell>();

            foreach (int id in _spells)
            {
                spells.Add(Core.Data.SpellLib.SpellLib.GetSpell(id));
            }

            _data.BasicSpells = spells.ToArray();

            _unit = UnitFactory.CreateUnit(_data, Team.Team.NONE, new Position() { Location = new Vector3(), ViewDirection = new Vector3() });

            _bar.SetActiveUnit(_unit);

            _unit.CastAbility(new EventData(_unit, _unit, _unit.GetAbility(Combat.Abilities.SpellSlot.FIRST).Spell));

            foreach (UnitView view in _setupUnits)
            {
                view.TryAssignTo(_unit);
            }
        }

        private void OnDestroy()
        {
            Engine.Combat.Stop();
            Engine.Combat.Reset();
        }
    }
}
