using UnityEngine;

namespace Data.Models
{
    public class Model
    {
        [SerializeField] private GameObject _defaultModel;
        //[SerializeField] private AnimationSet _animationSet;

        public GameObject GetDefaultModel() => _defaultModel;
    }
}
