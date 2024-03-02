using System;

namespace Client.Lobby.Core.Common
{
    public interface UpdateableModel
    {
        public event Action Updated;
    }
}
