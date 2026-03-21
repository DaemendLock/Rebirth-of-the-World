using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Domain.OldAttributes;

namespace TestSkillPack.Assets.Skills.TestSkillsPack.Summon
{
    [SkillScriptName("summon_test")]
    public class SummonSkill : SkillScript, ICastableSkill
    {
        public void OnCast(CastEvent @event)
        {
            CreateUnitInfo unitCreationData = new()
            {
                ModelName = new("Katerina"),
                Attributes = new AttributeSet()
                {
                    Haste = 100,
                    Speed = 5,
                }.ToAttributesArray(),
                Position = @event.Caster.Position + UnityEngine.Vector3.forward * 2,
                BaseHealth = 100,
            };

            @event.Scene.CreateUnit(unitCreationData);
        }
    }
}
