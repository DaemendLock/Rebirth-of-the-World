using Remaster.CombatSetup;
using Remaster.Data;
using Remaster.Engine;
using Remaster.View;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Remaster.Temp
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
                spells.Add(SpellLib.SpellLib.GetSpell(id));
            }

            _data.BasicSpells = spells.ToArray();

            _unit = UnitFactory.CreateUnit(_data, Team.NOTEAM, new Utils.Position() { Location = new Vector3(), ViewDirection = new Vector3() });

            _bar.SetActiveUnit(_unit);

            _unit.CastAbility(new Events.EventData(_unit, _unit, _unit.GetAbility(SpellSlot.FIRST).Spell));

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
