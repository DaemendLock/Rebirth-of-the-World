using Remaster.Networking.Protocol;

namespace Remaster.Net.Networking
{
    public static class Decoder
    {
        public static CastPayload ParseCastData(byte[] data)
        {
            int caster;
            int target;
            byte spellSlot;
            long time;

            return new CastPayload(caster, target, spellSlot, time);
        }

        
    }
}
