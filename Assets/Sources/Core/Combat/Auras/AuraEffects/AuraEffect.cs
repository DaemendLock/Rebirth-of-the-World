using Core.Combat.Utils;
using System;
using System.IO;
using Utils.Interfaces;

namespace Core.Combat.Auras.AuraEffects
{
    public interface AuraEffect : SerializableInterface
    {
        void ApplyEffect(Status status);
        void RemoveEffect(Status status);
    }

    #region CroudControl
    public class Stun : AuraEffect
    {
        public void ApplyEffect(Status status)
        {
            throw new NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }

    public class Fear : AuraEffect
    {
        public void ApplyEffect(Status status)
        {
            throw new NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }

    public class Root : AuraEffect
    {
        public void ApplyEffect(Status status)
        {
            throw new NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    public abstract class DynamicEffect : AuraEffect
    {
        protected readonly UnitAction Action;

        public DynamicEffect(UnitAction action)
        {
            Action = action;
        }

        public void ApplyEffect(Status status)
        {
            status.RegisterDynamicEffect(this, Action);
        }

        public void RemoveEffect(Status status)
        {
        }

        public abstract void Serialize(BinaryWriter buffer);

        public abstract void Update(Status status, CastEventData data);
    }
}
