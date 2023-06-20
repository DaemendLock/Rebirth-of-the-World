using Remaster.AuraEffects;
using Remaster.SpellEffects;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Remaster.Temp
{
    public class SpellIniter : MonoBehaviour
    {

        [SerializeField] private TestSpellDataCreator[] _spellData;

        private void Awake()
        {
            InitSpellEffects();

            foreach (TestSpellDataCreator data in _spellData)
            {
                data.Create();
            }
        }

        private void InitSpellEffects()
        {
            //Edge of faith
            _spellData[0].Effects = new SpellEffect[]
            {
                new ApplyAura(new ReactionCast(2, UnitAction.AUTOATTACK)),
                new ApplyAura(new ReactionCast(3, UnitAction.AUTOATTACK))
            };

            //Promising light
            _spellData[1].Effects = new SpellEffect[]
            {
                new Heal(new SpellpowerValue(1)),
            };

            //Edge hurm
            _spellData[2].Effects = new SpellEffect[]
            {
                new SchoolDamage(new SpellpowerValue(0.1f)),
            };

            //Edge self
            _spellData[3].Effects = new SpellEffect[]
            {
                new GiveResource(1, ResourceType.LIGHT_POWER),
                new Heal(new SpellpowerValue(0)),
            };

            _spellData[4].Effects = new SpellEffect[]
            {
                new SchoolDamage(new AttackpowerValue(1))
            };
        }


        private void OnDestroy()
        {
            // Logger.SaveLog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Log" + DateTime.Now.ToString().Replace(':', '.').Replace(' ', '_') + ".log");
            Logger.SaveLog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RotWLatestLog.log");
        }
    }
}
