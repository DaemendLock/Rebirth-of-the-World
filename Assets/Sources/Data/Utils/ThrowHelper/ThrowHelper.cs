using System.Runtime.CompilerServices;
using Utils.ThrowHepler.Exceptions;

namespace Data.Utils.ThrowHepler
{
    public static partial class ThrowHepler
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void CheckFileLoad(File file)
        {
            if (file == null || file.Disposed)
            {
                throw new FileNotLoadedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void CheckFileNotLoad(File file)
        {
            if (file != null && file.Disposed)
            {
                throw new FileLoadedException();
            }
        }
    }
}
