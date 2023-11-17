using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Data.Utils
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Implements <see cref="IDisposable"/> - Dispose after using.
    /// </remarks>
    /// 
    internal class File : IDisposable
    {
        private FileStream _source;
        private BinaryReader _stream;
        private bool _disposed;

        public File(string path)
        {
            _disposed = false;
            _source = System.IO.File.OpenRead(path);
            _stream = new BinaryReader(_source);
        }
        public bool Disposed => _disposed;

        ~File()
        {
            Dispose();
        }
       
        public bool HasNext => _source.Position < _source.Length;

        public void SetCursorPosition(long position)
        {
            _source.Position = position;
        }

        public byte[] ReadBytes(long count)
        {
            return _stream.ReadBytes((int) count);
        }

        public T ReadStruct<T>() where T : struct 
        {
            byte[] bytes = ReadBytes(Marshal.SizeOf<T>());

            GCHandle memory = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T result = (T) Marshal.PtrToStructure<T>(memory.AddrOfPinnedObject());
            memory.Free();

            return result;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _stream.Dispose();
            _source.Dispose();
            _disposed = true;

        }
    }
}
