using Data.Characters;
using Data.Entities;
using System.Collections.Generic;
using UnityEngine;
using static Utils.DataTypes.UnitCreationData;

namespace Client.Combat.View.Units
{
    public interface UnitView
    {
        void PerformCast()
        {
        }

        void StartPrecast()
        {
        }

        void SetHealth(float value)
        {

        }

        void UpdatePosition(Vector3 location, Vector3 velocity, float rotation);
    }

    public class Unit : MonoBehaviour, UnitView
    {
        private static Dictionary<int, Unit> _units = new();

        private Vector3 _velocity;

        private int _id;

        private Animator _animator;

        private void Update()
        {
            transform.position += _velocity * Time.deltaTime;
        }

        private void OnDestroy()
        {
            _units.Remove(_id);
        }

        public void Init(int id, ViewData data)
        {
            Character character = Character.Get(data.CharacterId);
            _id = id;
            UseModel(character.Npc.GetModel(0));

            _units[id] = this;
            gameObject.SetActive(true);
        }

        public int Id => _id;

        public static Unit GetUnit(int id)
        {
            return _units.GetValueOrDefault(id, null);
        }

        private void UseModel(NpcModel model)
        {
            model = Instantiate(model, transform);

            _animator = model.GetComponent<Animator>();
        }

        public void UpdatePosition(Vector3 location, Vector3 velocity, float rotation)
        {
            transform.position = location;
            //_velocity = velocity;
        }
    }
}
