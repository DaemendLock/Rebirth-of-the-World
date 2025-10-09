using System.Collections.Generic;

namespace CastStateSkill.Extenstions
{
    public static class SkillFrameDataExtension
    {
        public static SkillCastState GetFrameType(this IFrameData skillFrameData, float time) => skillFrameData.GetCastState((int) (time * FrameData.FrameRate));

        public static void Log(this FrameData skillFrameData)
        {
            int startupFrames = 0;
            int recoveryframes = 0;

            List<int> activeAndGaps = new();

            bool inGap = true;

            for (int i = 0; i < skillFrameData.TotalFrames; i++)
            {
                switch (skillFrameData.GetCastState(i / (float) FrameData.FrameRate))
                {
                    case SkillCastState.Startup:
                        startupFrames++;
                        break;

                    case SkillCastState.Recovery:
                        recoveryframes++;
                        break;

                    case SkillCastState.Active:
                        if (inGap)
                        {
                            activeAndGaps.Add(0);
                            inGap = false;
                        }

                        activeAndGaps[^1]++;
                        break;

                    case SkillCastState.Gap:
                        if (inGap == false)
                        {
                            activeAndGaps.Add(0);
                            inGap = true;
                        }

                        activeAndGaps[^1]++;
                        break;

                    default:
                        break;
                }
            }

            string result = string.Empty;

            result += $"<color=red>{new string('\x25A0', startupFrames)}</color>";

            for (int i = 0; i < activeAndGaps.Count; i++)
            {
                if ((i & 1) == 0)
                {
                    result += $"<color=green>{new string('\x25A0', activeAndGaps[i])}</color>";
                }
                else
                {
                    result += $"<color=gray>{new string('\x25A0', activeAndGaps[i])}</color>";
                }
            }

            result += $"<color=red>{new string('\x25A0', recoveryframes)}</color>";

            UnityEngine.Debug.Log(result);
        }
    }
}
