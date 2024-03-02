using System;
using System.Collections.Generic;

using UnityEngine;

using Data.Animations;
using Data.Sounds;

using Utils.DataTypes;

namespace Data.Entities
{
    [Serializable]
    public class ViewSet
    {
        [SerializeField] private NpcModel _model;
        [SerializeField] private Sprite _card;
        [SerializeField] private List<SpellView> _spellIcons;
        [SerializeField] private SoundPack _voiceover;
        [SerializeField] private AnimationPack _animationPack;

        public NpcModel Model => _model;
        public Sprite CardSprite => _card;
        public AnimationPack Animations => _animationPack;
        public Sprite GetSpellIcon(SpellId spellId) => _spellIcons.Find(value => value.SpellId == spellId).Icon;

    }

    [Serializable]
    public class SpellView
    {
        [SerializeField] private int _spellId;
        [SerializeField] private Sprite _icon;

        public Sprite Icon => _icon;
        public int SpellId => _spellId;
    }
}
