using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Units.ValueObjects;

using UnityEngine;

namespace Server.Combat.Networking
{
    public struct InputData
    {
        private Vector3 _position;
        private float _rotation;
        private ActionInputs _actions;
        private Vector3 _cursorPosition;
        private Quaternion _lookDirection;

        public InputData(EntityId id, Vector3 position, float rotation, Vector3 moveDirection, ActionInputs actionInputs)
        {
            ModelId = id;
            _position = position;
            _rotation = rotation;
            MoveDirection = moveDirection;

            _actions = actionInputs;

            _lookDirection = Quaternion.identity;
            _cursorPosition = Vector3.zero;
        }

        public EntityId ModelId { get; }

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public float Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public Vector3 MoveDirection { get; set; }

        public Quaternion LookDirection
        {
            get => _lookDirection;
            set => _lookDirection = value;
        }

        public ActionInputs Actions
        {
            get => _actions;
            set => _actions = value;
        }
    }
}