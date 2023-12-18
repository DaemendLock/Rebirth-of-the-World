using Data.Sounds;
using System;
using UnityEngine;

namespace Data.Characters
{
    [Serializable]
    public class ViewSet
    {
        [SerializeField] private GameObject _model;
        [SerializeField] private Sprite _characterCard;
        [SerializeField] private Sprite[] _spellIcons = new Sprite[6];
        [SerializeField] private SoundPack _voiceover;

        public GameObject Model => _model;

        public Sprite[] GetSpellIcons() => _spellIcons;

        public Sprite CharacterCard => _characterCard;
    }
}
