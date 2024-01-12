using Data.Animations;
using Data.Sounds;
using System;
using UnityEngine;

namespace Data.Entities
{
    [Serializable]
    public class ViewSet
    {
        [SerializeField] private NpcModel _model;
        [SerializeField] private Sprite _npcCard;
        [SerializeField] private Sprite[] _spellIcons = new Sprite[6];
        [SerializeField] private SoundPack _voiceover;
        [SerializeField] private AnimationPack _animationPack;

        public NpcModel Model => _model;

        public Sprite[] GetSpellIcons() => _spellIcons;

        public Sprite CharacterCard => _npcCard;

        public AnimationPack Animations => _animationPack;
    }
}
