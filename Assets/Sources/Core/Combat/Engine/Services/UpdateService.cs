using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Combat.Engine.Services
{
    internal interface IUpdateService
    {
        void Register(Updatable updatable);
        void Unregister(Updatable updatable);
        Task<ServerRecord> Update(long currentTime);
    }

    internal class FixedTimeUpdateService : IUpdateService
    {
        private List<Updatable> _updatables;
        private long _updateTime;
        private long _lastUpdate;

        public void Register(Updatable updatable)
        {
            if (_updatables.Contains(updatable))
            {
                throw new InvalidOperationException("Can't register same updatable twice.");
            }

            _updatables.Add(updatable);
        }

        public void Unregister(Updatable updatable)
        {
            if (_updatables.Remove(updatable) == false)
            {
                throw new InvalidOperationException("Unable to remove updatable.");
            }
        }

        public Task<ServerRecord> Update(long currentTime)
        {

            if (currentTime < _updateTime + _lastUpdate)
            {
                return null;
            }

            return Task.Run(UpdateAll);
        }

        private ServerRecord UpdateAll()
        {
            ServerRecord record = new();

            foreach (Updatable updatable in _updatables)
            {
                updatable.Update(record);
            }

            return record;
        }
    }
}
