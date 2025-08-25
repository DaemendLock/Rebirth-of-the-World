using Combat.Local.Domain.API;
using Combat.Local.Domain.API.Statuses;

namespace TestSkillPack.Assets.Skills.TestSkillsPack.SayHi
{
    [StatusScriptName("HiStatus")]
    public class SayHiStatus : StatusApi
    {
        public override void OnCreate()
        {
            StartPeriodicAction(0.5f / Source.Caster.GetHasteModifier());
        }

        public override void OnTick()
        {
            UnityEngine.Debug.Log(Parent.GetResourceValue(new(2)));
        }
    }
}
