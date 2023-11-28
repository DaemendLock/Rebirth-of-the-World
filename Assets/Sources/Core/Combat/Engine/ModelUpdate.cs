using System.Collections.Generic;

namespace Core.Combat.Engine
{
    public static class ModelUpdate
    {
        private static readonly List<Updatable> _updates = new();
        private static readonly Queue<Updatable> _registerQueue = new();

        public static long UpdateTime { get; private set; }

        public static void Update(float deltaTime)
        {
            if (Combat.Running)
            {
                return;
            }

            UpdateTime = (long) (deltaTime * 1000);

            UpdateModel();
            UpdateUpdateble();
            RegisterUpdateable();
        }

        internal static void Add(Updatable updatable)
        {
            _registerQueue.Enqueue(updatable);
        }

        internal static void Remove(Updatable updatable)
        {
            lock (_updates)
            {
                _updates.Remove(updatable);
            }
        }

        public static void Clear()
        {
            _registerQueue.Clear();
            _updates.Clear();
        }

        private static void UpdateModel()
        {
            //ModelSync.Synchronize();
        }

        private static void UpdateUpdateble()
        {
            foreach (Updatable updatable in _updates)
            {
                updatable.Update();
            }
        }

        private static void RegisterUpdateable()
        {
            lock(_updates)
            {
                _updates.Add(_registerQueue.Dequeue());
            }
        }
    }
}
