using System;
using UnityEditor.Animations;
using UnityEngine;

namespace Data.Animations
{
    [Serializable]
    public class AnimationPack
    {
        [SerializeField] private AnimatorController _controller;

        public AnimatorController Controller => _controller;
    }
}
