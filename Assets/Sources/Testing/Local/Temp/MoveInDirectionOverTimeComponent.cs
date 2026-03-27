using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using UnityEngine;

namespace Testing.Local.Temp
{
    public class MoveInDirectionOverTimeComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 _velocity;
        [SerializeField] private bool _isRelative;
        [SerializeField] private float _duration;

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (_duration <= 0)
            {
                enabled = false;
                return;
            }

            _duration -= Time.deltaTime;

            Vector3 velocity;

            if (_isRelative)
            {
                velocity = transform.rotation * _velocity;
            }
            else
            {
                velocity = _velocity;
            }

            Vector3 shift = velocity * Time.deltaTime;

            transform.position += shift;
        }
    }
}
