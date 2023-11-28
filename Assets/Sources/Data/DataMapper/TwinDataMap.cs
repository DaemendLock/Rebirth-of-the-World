using Data.DataMapper;

namespace Data.Spells
{
    internal readonly struct TwinDataMap
    {
        public readonly MappedData First;
        public readonly MappedData Second;

        public TwinDataMap(MappedData value1, MappedData value2)
        {
            First = value1;
            Second = value2;
        }
    }
}
