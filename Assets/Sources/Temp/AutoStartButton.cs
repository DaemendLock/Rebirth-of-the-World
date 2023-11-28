using Assets.Sources.Temp.Template;
using Data.Model;
using System;
using Temp.Testing;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using View.General;

namespace Assets.Sources.Temp
{
    internal class AutoStartButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private EncounterTemplate _encounter;

        public void OnPointerClick(PointerEventData eventData)
        {
            Loader.LoadScene(1);
            CombatIniter.Init();
            _encounter.LoadChractersToCombat();
        }
    }
}
