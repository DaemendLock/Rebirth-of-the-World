using System;

namespace Utils.ThrowHepler.Exceptions
{
    public class FileLoadedException : Exception
    {
        public FileLoadedException() : base("File already loaded.") { }
    }
}
