using Utils.ThrowHepler.Exceptions;

namespace Data.Utils.ThrowHepler
{
    public static partial class ThrowHepler
    {
        internal static void CheckFileLoad(File file)
        {
            if (file == null || file.Disposed)
            {
                throw new FileNotLoadedException();
            }
        }

        internal static void CheckFileNotLoad(File file)
        {
            if (file != null && file.Disposed)
            {
                throw new FileLoadedException();
            }
        }
    }
}
