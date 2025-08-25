namespace Server.Combat.Networking
{
    public readonly struct ActionInputs
    {
        private readonly int _values;

        public ActionInputs(int values)
        {
            _values = values;
        }

        public bool GetStatus(int index) => ((_values >> index) & 1) != 0;
    }
}