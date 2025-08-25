using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Units.ValueObjects;

using UnityEngine;

namespace Assets.Sources.Temp
{
    public struct UnitDTO
    {
        public EntityId Id { get; }
        public UnitId UnitId {get;}
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public bool Alive { get; set; }
        public float CurrentHealth { get; }
        public int CastId { get; set; }
        public float CastTime { get; set; }
    }

    public class ServerClientSyncer
    {


        public void UpdateClientFromServerData()
        {
            //Send server position, active skill, health


        }
    }
}
