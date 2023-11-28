using System;

namespace Utils.ThrowHepler
{
    public static partial class ThrowHepler
    {
        public static void CheckForNull(object @object)
        {
            if (@object == null)
            {
                throw new ArgumentException("Object is null");
            }
        }
    }
}
