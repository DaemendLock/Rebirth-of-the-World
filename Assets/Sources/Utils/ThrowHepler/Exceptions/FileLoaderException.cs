using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.ThrowHepler.Exceptions
{
    internal class FileLoaderException : Exception
    {
        FileLoaderException() : base("File already loaded.") { }
    }
}
