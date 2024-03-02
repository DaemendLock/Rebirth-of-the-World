using Client.Lobby.Core.Characters;
using Client.Lobby.View.Utils;
using System;
using UnityEngine;

namespace Client.Lobby.View.Tooltip
{
    internal class Tooltip : MenuElement
    {
        private static Tooltip _instance;

        public static Tooltip Instance
        {
            get => _instance;
            set
            {
                if (_instance != null)
                {
                    value.enabled = false;
                    throw new InvalidOperationException($"Instance of {typeof(Tooltip)} already exists.");
                }

                _instance = value;
            }
        }

        public static void Show(Spell item)
        {
            _instance?.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            _instance?.gameObject.SetActive(false);
        }
    }
}
