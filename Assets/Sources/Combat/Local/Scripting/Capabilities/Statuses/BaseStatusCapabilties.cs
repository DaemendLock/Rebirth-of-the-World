using Combat.API.Scripting;

namespace Combat.Local.Scripting.Capabilities.Statuses
{
    public readonly ref struct BaseStatusCapabilties
    {
        private readonly StatusScript _statusScript;

        public BaseStatusCapabilties(StatusScript statusScript)
        {
            _statusScript = statusScript;
        }

        public bool DestroyOnExpire => true;

        public void Apply() => _statusScript.OnCreate();

        public void Expire() => _statusScript.OnExpire();

        public void Remove() => _statusScript.OnRemove();

        public void Tick() => _statusScript.OnTick();
    }
}
