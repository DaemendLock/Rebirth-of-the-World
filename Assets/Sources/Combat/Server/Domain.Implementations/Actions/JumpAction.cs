using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.Implementations.Actions
{
    public class JumpAction // : IAction
    {
        private float _jumpSpeed;

        public JumpAction(float jumpSpeed)
        {
            _jumpSpeed = jumpSpeed;
        }

        //TODO: if on ground or can extraJump
        //public bool CanPerformBy(Unit actor) => actor.CanMove();

        public void PerformBy(Unit actor)
        {
           // actor.Velocity += UnityEngine.Vector3.up * _jumpSpeed;
        }
    }
}
