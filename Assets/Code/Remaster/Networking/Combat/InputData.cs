using Remaster.Networking.Protocol;
using System;
namespace Remaster.Networking.Combat
{
    public interface InputData : PayloadProvider
    {
    }

    public sealed class CastRequest : InputData
    {
        private Unit _caster;
        private Unit _target;
        private SpellSlot _slot;

        public CastRequest(Unit caster, Unit target, SpellSlot slot)
        {
            _caster = caster;
            _target = target;
            _slot = slot;
        }

        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class MoveTowardRequest : InputData
    {
        private Unit _unit;

        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class TargetRquest : InputData
    {
        private Unit _attacker;
        private Unit _target;

        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }
    }
}
