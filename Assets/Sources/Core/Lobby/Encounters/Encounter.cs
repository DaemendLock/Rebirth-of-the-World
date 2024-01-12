using Data.Characters;
using Data.Entities;
using System;
using UnityEngine;
using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Lobby.Encounters
{
    [Serializable]
    public class Encounter
    {
        [SerializeField] private string _name;
        [SerializeField] private int _playerCount;

        [SerializeField] private EncounterGroup[] _enemies;

        public Encounter(string name, int playerCount)
        {
            _name = name;
            _playerCount = playerCount;
        }

        public string Name => _name;

        public int PlayerCount => _playerCount;

        public EncounterGroup[] EncounterGroups => _enemies;

        [Serializable]
        public class EncounterGroup
        {
            [SerializeField] private EncounterNpc[] _npcs;

            public EncounterNpc[] NPCs => _npcs;
        }

        [Serializable]
        public class EncounterNpc
        {
            [SerializeField] private Npc _npc;
            [SerializeField] private byte _team;
            [SerializeField] private UnityEngine.Vector3 _location;
            [Range(0, 360)]
            [SerializeField] private float _rotation;
            [SerializeField] private CharacterState _state;

            public UnitCreationData GetUnitCreationData(int index, byte contolGroup)
            {
                StatsTable stats = _npc.GetStatsTable(0);

                UnitCreationData.ModelData mdata = new UnitCreationData.ModelData(new SpellId[0], stats, new (new(_location.x, _location.y, _location.z), _rotation), _npc.CastResources,
                    _team);
                UnitCreationData.ViewData vdata = new UnitCreationData.ViewData(_state.CharacterId, _state.ViewSet);
                UnitCreationData udata = new UnitCreationData(index, mdata, vdata, contolGroup);

                return udata;
            }
        }
    }
}
