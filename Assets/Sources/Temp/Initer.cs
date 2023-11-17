using Assets.Sources.Temp;
using Assets.Sources.Temp.Template;
using Core.Combat.Engine;
using Core.Combat.Utils;
using Core.SpellLib.Paladin;
using Core.SpellLibrary;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using View.Combat.UI.Nameplates;
using View.Combat.Units;

namespace Temp.Testing
{
    internal class Initer : MonoBehaviour
    {
        private Thread _update;

        [SerializeField] private Nameplate _nameplatePrefab;
        [SerializeField] private NameplatesRoot _nameplatesRoot;

        [SerializeField] private List<ModelLoadData> _loadModels;

        private void Awake()
        {
#if DEBUG
            Combat.DebugMessage += Debug.Log;
#endif
            SpellLib.LoadAllData();

            //Paladin
            new LifegivingLight();
            new BladeOfFaith();
            new BladeOfFaithProc();
            new BladeOfFaithProcSelf();
            new Consecration();
            new ConsecrationAllyBuff();
            new ConsecrationEnemyDamage();
            Debug.Log(new CandentArmor());
            Debug.Log(new CandentArmorProc());
            Debug.Log(new CandentArmorProcPower());

            CombatTime.SetStartTime(Environment.TickCount);

            _update = new (new ThreadStart(CombatInitializer.Initialize));
            _update.Priority = System.Threading.ThreadPriority.Highest;
            _update.Start();
            
            NameplatesRoot.NameplatePrefab = _nameplatePrefab;
            InitModelLib();

            Networking.Combat.UseClient(new AutoAcceptClient());
        }

        private void OnDestroy()
        {
            Combat.Stop();
        }

        private void Update()
        {
            Combat.Update(UnityEngine.Time.deltaTime);
        }

        private void InitModelLib()
        {
            foreach (ModelLoadData data in _loadModels)
            {
                ModelLibrary.LoadModel(data.Name, data.Model);
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
