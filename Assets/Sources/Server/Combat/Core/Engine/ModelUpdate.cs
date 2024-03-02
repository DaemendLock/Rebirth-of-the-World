using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine.Services;
using System.Collections.Generic;
using System.IO;

namespace Core.Combat.Engine
{
    internal interface Updatable
    {
        void Update(IActionRecordContainer container);
    }

    public class UpdateRecord : IActionRecordContainer
    {
        public readonly long Time;
        private readonly IActionRecordContainer _actions;
        private readonly List<PositionUpdateRecord> _positions;

        public UpdateRecord(long time, List<PositionUpdateRecord> memory)
        {
            Time = time;
            _actions = new ActionRecordContainer();
            memory.Clear();
            _positions = memory;
        }

        public void AddAction(ActionRecord record) => _actions.AddAction(record);

        public void AddPositionUpdate(PositionUpdateRecord value) => _positions.Add(value);

        public void Write(BinaryWriter target)
        {
            target.Write(Time);

            target.Write(_positions.Count);

            foreach (PositionUpdateRecord item in _positions)
            {
                target.Write(item.UnitId);
                target.Write(item.Position.Location.x);
                target.Write(item.Position.Location.y);
                target.Write(item.Position.Location.z);
                target.Write(item.Position.Rotation);
                target.Write(item.Velocity.x);
                target.Write(item.Velocity.y);
                target.Write(item.Velocity.z);
            }

            _actions.Write(target);
        }
    }

    public interface InputRequest
    {
        bool IsValid();
        void Perform(UpdateRecord target);
    }

    public static class ModelUpdate
    {
        private static readonly MemoryStream _memory = new MemoryStream();
        private static readonly UpdateService _updateService = new DynamicTimeUpdateService();
        private static readonly Queue<InputRequest> _inputs = new();

        public static long UpdateTime => _updateService.UpdateTime;
        public static long LastUpdate => _updateService.LastUpdate;

        public static UpdateRecord Update(long deltaTime)
        {
            if (Combat.Running == false)
            {
                return null;
            }

            return _updateService.Update(deltaTime, _memory);
        }

        public static void UpdateInput(UpdateRecord target)
        {
            while (_inputs.Count > 0)
            {
                _inputs.Dequeue().Perform(target);
            }
        }

        public static void RegisterInput(InputRequest input)
        {
            _inputs.Enqueue(input);
        }
    }
}
