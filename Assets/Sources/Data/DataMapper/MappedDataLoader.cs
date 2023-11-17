using Data.Utils;
using Data.Utils.ThrowHepler;
using System;

namespace Data.DataMapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Implements IDisposable. Release unmanaged memory after using.
    /// </remarks>
    internal class MappedDataLoader : IDisposable
    {
        private string _source;
        private File _file;

        public MappedDataLoader(string source)
        {
            _source = source;
        }

        ~MappedDataLoader()
        {
            Release();
        }

        public void Load()
        {
            _file ??= FileReader.ReadFile(_source);
        }

        public void Release()
        {
            _file?.Dispose();
            _file = null;
        }

        public byte[] GetBytes(MappedData map)
        {
            ThrowHepler.CheckFileLoad(_file);
            _file.SetCursorPosition(map.Position);

            return _file.ReadBytes(map.Size);
        }

        public void Dispose()
        {
            Release();
        }
    }
}
