using Combat.API;
using Combat.API.DTO;
using Combat.API.Skills;
using Combat.Domain.OldAttributes;

namespace TestSkillPack.Assets.Skills.TestSkillsPack.Summon
{
    [SkillScriptName("summon_test")]
    public class SummonSkill : SkillScript, ICastableSkill
    {
        public void OnCast()
        {
            UnitCreationData unitCreationData = new()
            {
                ModelName = new("Katerina"),
                Attributes = new AttributeSet()
                {
                    Haste = 100,
                    Speed = 5,
                }.ToAttributesArray(),
                Position = Owner.Position + UnityEngine.Vector3.forward * 2,
                BaseHealth = 100,
            };

            Enviroment.CreateUnit(unitCreationData);
        }
    }
}
