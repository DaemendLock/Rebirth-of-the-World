namespace Data.DataMapper
{
#if UNITY_EDITOR
    public
#else
    internal  
#endif
readonly struct MappedData
    {
        public readonly long Position;
        public readonly long Size;

        public MappedData(long position, long size)
        {
            Position = position;
            Size = size;
        }
    }
}
