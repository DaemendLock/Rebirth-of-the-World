using System;

namespace UtilsUnity.ThrowHepler.Exceptions
{
    public class FileLoadedException : Exception
    {
        public FileLoadedException() : base("File already loaded.") { }
    }
}
