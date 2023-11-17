using System.Collections.Generic;
using UnityEngine;

namespace View.Combat.Units
{
    public class Unit : MonoBehaviour
    {
        //[SerializeField] private Model.Model _model;

        //[SerializeField] private AnimationController _animator;
        //[SerializeField] private Nameplate _nameplate;
        private static Dictionary<int, Unit> _units = new();

        //private Vector3 _moveDirection;
        //private bool _isMoving;

        private Core.Combat.Units.Unit _assignedUnit;

        public int Id { get; private set; }

        private void Update()
        {
            Utils.DataTypes.Vector3 pos = _assignedUnit.Position;
            transform.position = new(pos.x, pos.y, pos.z);

            Utils.DataTypes.Vector3 rotation = new(_assignedUnit.Rotation);

            transform.forward = new Vector3(rotation.x, rotation.y, rotation.z);
        }

        private void OnDestroy()
        {
            if (_assignedUnit == null)
            {
                return;
            }

            _units.Remove(Id);
        }

        public void Init(int id, GameObject model   )
        {
            if (_assignedUnit != null)
            {
                return;
            }

            _assignedUnit = Core.Combat.Engine.Combat.GetUnit(id);
            Id = id;

            gameObject.SetActive(true);

            Instantiate(model, transform);

            _units[id] = this;
        }

        public void ChangeModel(string model)
        {

        }

        public void PlayAnimation(Animations.Animation animation)
        {
            //animation.Apply(_animator);
        }

        public void StopAllActions()
        {
            //_isMoving = false;
        }

        public void FaceInDirection(Vector3 direction)
        {

        }

        public void LookAt(Vector3 point)
        {

        }

        public void MoveInDirection(Vector3 direction)
        {
            //_isMoving = true;
            //_moveDirection = direction.normalized;
        }

        public static Unit GetUnit(int id)
        {
            return _units.GetValueOrDefault(id, null);
        }
    }
}
