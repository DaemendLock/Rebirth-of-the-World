using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

using System;

namespace Combat.Local.Domain.UseCases.Players
{
    public class UnityDebugLogPlayerOutput : IPlayerCreateOutput
    {
        public void Present(Player player) =>
            UnityEngine.Debug.Log("New player for input readed: " + player.Id);
    }

    public interface IPlayerCreateOutput
    {
        void Present(Player player);
    }
}
