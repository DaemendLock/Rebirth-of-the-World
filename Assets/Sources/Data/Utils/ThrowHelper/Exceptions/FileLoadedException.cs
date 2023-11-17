using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.ThrowHepler.Exceptions
{
    public class FileLoadedException : Exception
    {
        public FileLoadedException() : base("File already loaded.") { }
    }
}
