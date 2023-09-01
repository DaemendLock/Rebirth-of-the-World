using Core.Networking.Protocol;

namespace Core.Net.Networking
{
    public static class Decoder
    {
        public static CastPayload ParseCastData(byte[] data)
        {
            int caster = 0;
            int target = 0;
            byte spellSlot = 0;
            long time = 0;

            return new CastPayload(caster, target, spellSlot, time);
        }
    }
}
