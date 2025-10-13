using Combat.API.Scripting;
using Combat.API.Statuses;

namespace TestSkillPack.Assets.Skills.TestSkillsPack.SayHi
{
    [StatusScriptName("HiStatus")]
    public class SayHiStatus : StatusScript
    {
        public override void OnCreate()
        {
            Instance.StartPeriodicAction(0.5f / Source.Owner.GetHasteModifier());
        }

        public override void OnTick()
        {
            UnityEngine.Debug.Log(Parent.GetResourceValue(new(2)));
        }
    }
}
