using UnityEngine;

namespace Client.Combat.Presentation.Assets.Sources.Client.Combat.View
{
    [CreateAssetMenu(fileName = "Data", menuName = "Unit Animations/Spell Animation", order = 1)]
    public class SpellAnimation : ScriptableObject
    {
        [SerializeField] private AnimationClip _animation;

        [Header("Frame data")]
        [SerializeField] private ushort _startup;
        [SerializeField] private ushort _active;
        [SerializeField] private ushort _recovery;

        public float StartupTime => _startup / 60f;
        public float ActiveTime => _active / 60f;
        public float RecoveryTime => _recovery / 60f;

        public string Animation => _animation.name;
    }
}
