using Combat.API.Scripting;

using Combat.Local.Data.Databases;

using System;
using System.Runtime.Serialization;

namespace Combat.Local.Data.Factories
{
    public class SkillScriptFactory
    {
        private readonly SkillStrategyTypeProvider _skillScriptTypeProvider;

        public SkillScriptFactory(SkillStrategyTypeProvider skillScriptTypeProvider)
        {
            _skillScriptTypeProvider = skillScriptTypeProvider;
        }

        public SkillScript Create(string scriptName)
        {
            if (_skillScriptTypeProvider.TryGet(scriptName, out Type type) == false)
            {
                throw new InvalidOperationException($"No script assigned found with name \"{scriptName}\".");
            }

            return (SkillScript)FormatterServices.GetUninitializedObject(type);
        }
    }
}
