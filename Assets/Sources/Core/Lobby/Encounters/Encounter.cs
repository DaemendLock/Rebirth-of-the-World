﻿using Core.Combat.Team;
using Core.Combat.Units.Components;
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

        [SerializeField] private EncounterNpc[] _enemies;

        public Encounter(string name, int playerCount)
        {
            _name = name;
            _playerCount = playerCount;
        }

        public string Name => _name;

        public int PlayerCount => _playerCount;

        public EncounterNpc[] EncounterUnits => _enemies;

        [Serializable]
        public class EncounterNpc
        {
            [SerializeField] private Npc _npc;
            [SerializeField] private Team _team;
            [SerializeField] private UnityEngine.Vector3 _location;
            [Range(0, 360)]
            [SerializeField] private float _rotation;
            [SerializeField] private CharacterState _state;

            public UnitCreationData GetUnitCreationData(int index)
            {
                StatsTable stats = _npc.GetStatsTable(0);

                UnitCreationData.ModelData mdata = new UnitCreationData.ModelData(new SpellId[0], stats, new (new(_location.x, _location.y, _location.z), _rotation), _npc.CastResources,
                    (byte)_team);
                UnitCreationData.ViewData vdata = new UnitCreationData.ViewData(_state.CharacterId, _state.ViewSet);
                UnitCreationData udata = new UnitCreationData(index, mdata, vdata);

                return udata;
            }
        }
    }
}
