using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Combat.Engine
{
    internal interface Updatable
    {
        void Update(IActionRecordContainer container);
    }

    public class ServerRecord : IActionRecordContainer
    {
        public readonly int Tick;
        private readonly List<ActionRecord> _actions;

        public void AddAction(ActionRecord record)
        {
            _actions.Add(record);
        }
    }

    public static class ModelUpdate
    {
        private static readonly List<Updatable> _updates = new();
        private static readonly Queue<Updatable> _registerQueue = new();

        private static readonly IUpdateService _updateService = new FixedTimeUpdateService();

        public static long UpdateTime { get; private set; }
        private static long _lastUpdate;

        public static async Task<ServerRecord> Update(float deltaTime)
        {
            if (Combat.Running == false)
            {
                return null;
            }

            RegisterUpdateable();
            return await _updateService.Update(_lastUpdate + (long) (deltaTime * 1000));
        }

        internal static void Add(Updatable updatable)
        {
            _updateService.Register(updatable);
        }

        internal static void Remove(Updatable updatable)
        {
            lock (_updates)
            {
                _updateService.Unregister(updatable);
            }
        }

        public static void Clear()
        {
            _registerQueue.Clear();
            _updates.Clear();
        }

        private static void RegisterUpdateable()
        {
            if (_registerQueue.Count == 0)
            {
                return;
            }

            lock (_updates)
            {
                _updates.Add(_registerQueue.Dequeue());
            }
        }
    }
}
