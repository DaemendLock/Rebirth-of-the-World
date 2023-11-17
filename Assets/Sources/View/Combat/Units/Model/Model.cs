using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using View.Combat.Units.Animations;

namespace View.Combat.Units.Model
{
    public class Model : MonoBehaviour
    {
        [SerializeField] private GameObject _defaultModel;

        public GameObject GetPoint()
        {
            return _defaultModel;
        }
    }
}
