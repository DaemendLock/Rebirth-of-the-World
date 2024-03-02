using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine;
using System.IO;
using Utils.ByteHelper;

namespace Server.Combat
{
    public static class CombatServer
    {
        private static readonly RequestAdapter _userInputAdapter = new();
        private static readonly ServerOutputModule _out = new();

        public static void Handle(byte[] data)
        {
            ByteReader source = new ByteReader(data);

            InputRequest request = _userInputAdapter.Adapt(source);

            if (request.IsValid() == false)
            {
                //_out.InformRequestClient(new WrongRequest);
                return;
            }

            ModelUpdate.RegisterInput(request);
        }

        public static void Update(float time)
        {
            _out.SendToAllClients(ModelUpdate.Update((long) (time * 1000)));
        }
    }

    public class ServerOutputModule
    {
        public void SendToAllClients(IActionRecordContainer record)
        {
            using MemoryStream stream = new MemoryStream();
            using (BinaryWriter packet = new BinaryWriter(stream))
            {
                record.Write(packet);
            }

            // Send to all clients new packet
            // ServerCommandsAdapter.HandleUpdate(stream.ToArray());
        }
    }
}