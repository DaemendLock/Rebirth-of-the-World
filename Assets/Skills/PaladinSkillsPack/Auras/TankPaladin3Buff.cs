using Combat.API.Scripting;
using Combat.API.Statuses;

namespace TestSkillsPack.Paladin
{
    [StatusScriptName("TankPaladin3Buff")]
    public class TankPaladin3Buff : StatusScript, IMovespeedModifier
    {
        float IMovespeedModifier.GetBonusMovespeedPercent() => 20f;
    }
}
