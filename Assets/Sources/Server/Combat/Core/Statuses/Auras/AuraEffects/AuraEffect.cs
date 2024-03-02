using Core.Combat.Units;
using Utils.Interfaces;

namespace Core.Combat.Statuses.Auras.AuraEffects
{
    public interface AuraEffect : SerializableInterface
    {
        void ApplyEffect(Unit target);

        void RemoveEffect(Unit target);
    }

    //#region CroudControl
    //public class Stun : AuraEffect
    //{
    //    public void ApplyEffect(Status status)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void RemoveEffect(Status status)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Serialize(BinaryWriter buffer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class Fear : AuraEffect
    //{
    //    public void ApplyEffect(Status status)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void RemoveEffect(Status status)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Serialize(BinaryWriter buffer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class Root : AuraEffect
    //{
    //    public void ApplyEffect(Status status)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void RemoveEffect(Status status)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Serialize(BinaryWriter buffer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    //#endregion
}
