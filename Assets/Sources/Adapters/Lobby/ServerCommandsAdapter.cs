using Core.Lobby.Chat;
using View.General;

namespace Adapters.Lobby
{
    public static class ServerCommandsAdapter
    {
        public static void HandleCommand(byte[] data)
        {
            switch (data[0])
            {
                case (byte) ServerCommand.ShowChatMessage:
                    HandleChatMessage(data);
                    return;

                case (byte) ServerCommand.UpdateCharacterData:
                    HandleCharacterData(data);
                    return;

                case (byte) ServerCommand.UpdateUserData:
                    return;
            }
        }

        private static void HandleChatMessage(byte[] data)
        {
            ShowChatMessageCommand mdata = ShowChatMessageCommand.Parse(data, 1);

            Chat.PostMessage(mdata.ChannelId, mdata.AuthorId, mdata.Message);
        }

        private static void HandleCharacterData(byte[] data)
        {
            //ShowChatMessageCommand mdata = ShowChatMessageCommand.Parse(data, 1);
            UpdateCharacterDataCommand udata = UpdateCharacterDataCommand.Parse(data, 1);

            //CharactersList.UpdateCharacterData(udata.CharacterId, udata.Data);
        }

        private static void HandleUserData(byte[] data)
        {
            //ShowChatMessageCommand mdata = ShowChatMessageCommand.Parse(data, 1);
            UpdateCharacterDataCommand udata = UpdateCharacterDataCommand.Parse(data, 1);

            //CharactersList.UpdateCharacterData(udata.CharacterId, udata.Data);
        }

        private static void SetupEncounterData(byte[] data)
        {

            //View.Lobby.Lobby.Instance?.SetupEncounter();
        }

        private static void StartCombat(byte[] data)
        {
            StartCombatData cdata = StartCombatData.Parse(data, 1);

            SceneLoader.Combat.Load();
        }
    }
}
