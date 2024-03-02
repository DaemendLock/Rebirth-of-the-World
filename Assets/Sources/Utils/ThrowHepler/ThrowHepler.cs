using System;

namespace Utils.ThrowHepler
{
    public static partial class ThrowHepler
    {
        public static void ArgumentNullException(params object[] args)
        {
            for (int i = 0; i < args.Length;i++)
            {
                if (args[i] == null)
                {
                    throw new ArgumentNullException("", $"Given parameter at index {i} can't be null.");
                }
            }
        }
    }
}
