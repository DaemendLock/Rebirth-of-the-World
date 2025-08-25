using System.Collections.Generic;

using Server.Combat.Infrastructure.Controllers;
using Server.Combat.Infrastructure.Repositories;
using Server.Combat.Networking;

using Zenject;

namespace Assets.Sources.Temp
{
    public class ClientInputReader : ITickable
    {
        private readonly HashSet<InputData> _inputBuffer;
        private readonly Dictionary<int, InputData> _records;
        private readonly IUnitControllerRepository _unitControllerRepository;

        public ClientInputReader(IUnitControllerRepository unitControllerRepository)
        {
            _unitControllerRepository = unitControllerRepository;

            _inputBuffer = new();
            _records = new Dictionary<int, InputData>();
        }

        public void AddInputData(InputData inputData) => _inputBuffer.Add(inputData);

        public void Tick()
        {
            foreach (InputData data in _inputBuffer)
            {
                HandleInputUpdate(data);
            }

            _inputBuffer.Clear();
        }

        public void HandleInputUpdate(InputData newInput)
        {
            IUnitController unitController = _unitControllerRepository.Get(newInput.ModelId);

            if (unitController == null)
            {
                return;
            }

            if (_records.TryGetValue(newInput.ModelId.Value, out InputData record) == false)
            {
                record = new();
            }

            if (newInput.MoveDirection.sqrMagnitude > 1)
            {
                newInput.MoveDirection = newInput.MoveDirection.normalized;
            }

            unitController.TryMove(newInput.Position, newInput.Rotation, newInput.MoveDirection);

            for (int i = 0; i < 16; i++)
            {
                if ((record.Actions.GetStatus(i) == false) && newInput.Actions.GetStatus(i))
                {
                    unitController.PerformAction(i);
                }
            }

            _records[newInput.ModelId.Value] = newInput;
        }
    }
}
