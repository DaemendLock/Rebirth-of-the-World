using System;

namespace UtilsUnity.ThrowHepler.Exceptions
{
    internal class FileNotLoadedException : Exception
    {
        public FileNotLoadedException() : base("File not loaded") { }
    }
}
