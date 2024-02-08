using System;

namespace Utils.ThrowHepler
{
    public static partial class ThrowHepler
    {
        public static void ArgumentNullException(params object[] args)
        {
            foreach (object arg in args)
            {
                if (arg == null)
                {
                    throw new ArgumentNullException();
                }
            }
        }
    }
}
