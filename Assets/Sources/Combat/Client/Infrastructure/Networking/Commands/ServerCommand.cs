namespace Client.Combat.Infrastructure.Networking.Commands
{
    public interface ServerCommand
    {
        void Perform();
    }

    public enum ServerCommandType : byte
    {
        PrintDebug,
        Update,
    }

    public class UpdateCommand : ServerCommand
    {
        public void Perform()
        {

        }
    }
}
