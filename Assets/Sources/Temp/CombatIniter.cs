using Assets.Sources.Temp;
using Core.Combat.Abilities;
using Core.Combat.Engine;
using Core.Combat.Utils;
using Core.SpellLib.Paladin;
using Data.Spells;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Utils.DataTypes;

namespace Temp.Testing
{
    internal class CombatIniter : MonoBehaviour
    {
        private Thread _update;

        [SerializeField] private List<ModelLoadData> _loadModels;

        public static void Init()
        {
#if DEBUG
            Combat.DebugMessage += Debug.Log;
#endif
            //Paladin
            Spell.RegisterSpell(new LifegivingLight());
            Spell.RegisterSpell(new BladeOfFaith());
            Spell.RegisterSpell(new BladeOfFaithProc());
            Spell.RegisterSpell(new BladeOfFaithProcSelf());
            Spell.RegisterSpell(new Consecration());
            Spell.RegisterSpell(new ConsecrationAllyBuff());
            Spell.RegisterSpell(new ConsecrationEnemyDamage());
            Spell.RegisterSpell(new CandentArmor());
            Spell.RegisterSpell(new CandentArmorProc());
            Spell.RegisterSpell(new CandentArmorProcPower());

            CombatTime.SetStartTime(Environment.TickCount);
            Combat.Start();
            //_update.Priority = System.Threading.ThreadPriority.Highest;
            //_update.Start();

            InitSpellLibrary();

            Networking.Combat.UseClient(new AutoAcceptClient());
        }

        private void Awake()
        {

        }

        private void OnDestroy()
        {
            Core.Combat.Engine.Combat.Stop();
        }

        private void Update()
        {
            ModelUpdate.Update(UnityEngine.Time.deltaTime);
        }

        private static void InitSpellLibrary()
        {
            SpellDataLoader.Load();

            SpellId[] ids = SpellDataLoader.GetLoadedIds();

            foreach (SpellId id in ids)
            {
                Spell.Get(id);
            }
        }
    }

    [Serializable]
    public class ModelLoadData
    {
        public string Name;
        public GameObject Model;
    }
}
