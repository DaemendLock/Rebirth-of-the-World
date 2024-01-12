using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
