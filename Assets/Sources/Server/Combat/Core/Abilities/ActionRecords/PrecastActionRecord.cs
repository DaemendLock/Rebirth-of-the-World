using Utils.DataTypes;

namespace Core.Combat.Abilities.ActionRecords
{
    internal class PrecastActionRecord : ActionRecord
    {
        private readonly int _casterId;

        public PrecastActionRecord(int casterId, int targetId, SpellId source, float time)
        {
            _casterId = casterId;
            Target = targetId;
            Source = source;
            Time = time;
        }

        public ActionType Type => ActionType.Precast;

        public int Target { get; }

        public SpellId Source { get; }

        public float Time { get; }

        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
