using Lobby.Common.Primitives;

using System;
using System.Numerics;

namespace Combat.Local.Domain.Entities
{
    public readonly struct Spawnpoint
    {
        public readonly Vector3 Position;
    }

    public readonly ref struct Location
    {
        public readonly LocationId Id;
        public readonly ReadOnlySpan<Spawnpoint> Spawnpoints;
    }

    public ref struct Encounter
    {
        public Location Location { get; set; }
    }
}
