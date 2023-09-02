using Core.Combat.Abilities;
using Core.Data.SpriteLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Temp
{
    public class SpellIniter : MonoBehaviour
    {
        private List<Spell> _spellData = new();

        private void Awake()
        {
            InitSpellEffects();

            foreach (Spell data in _spellData)
            {
                SpellLibrary.SpellLib.RegisterSpell(data);
                SpriteLibrary.LoadSpell(data.Id);
                Debug.Log(data.Id);
            }
        }

        private void InitSpellEffects()
        {
            _spellData.Add(new SpellLib.Warrior.DirectHit());
            _spellData.Add(new SpellLib.Warrior.СoncentratedDefense());
            _spellData.Add(new SpellLib.Warrior.Slash());
            _spellData.Add(new SpellLib.Warrior.IgnorPain());
            _spellData.Add(new SpellLib.Warrior.WillForVictory());
            _spellData.Add(new SpellLib.Warrior.Charge());
        }

        private void OnDestroy()
        {
            // Logger.SaveLog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Log" + DateTime.Now.ToString().Replace(':', '.').Replace(' ', '_') + ".log");
            Utils.Logger.Logger.SaveLog(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RotWLatestLog.log");
        }
    }
}
