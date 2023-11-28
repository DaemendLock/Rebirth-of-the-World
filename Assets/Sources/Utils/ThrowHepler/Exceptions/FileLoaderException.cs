using System;

namespace Utils.ThrowHepler.Exceptions
{
    internal class FileLoaderException : Exception
    {
        private FileLoaderException() : base("File already loaded.") { }
    }
}
