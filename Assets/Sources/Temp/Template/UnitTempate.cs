using Assets.Sources.Temp.Template;
using Core.Combat.Stats;
using Core.Combat.Team;
using Core.Combat.Units;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utils.DataTypes;

namespace Temp.Template
{
    internal class UnitTempate : MonoBehaviour
    {
        [SerializeField] private int _id = 0;

        [SerializeField] private List<int> _spellIds = new();

        [SerializeField] private PercentModifiedValue[] _stats = new PercentModifiedValue[StatsTable.STATS_COUNT];

        [SerializeField] private float _leftResourceMax;
        [SerializeField] private float _rightResourceMax;
        [SerializeField] private ResourceType _leftType;
        [SerializeField] private ResourceType _rightType;

        [SerializeField] private UnityEngine.Vector3 _startPostion;
        [SerializeField] private float _rotation;

        [SerializeField] private Team _team;

        [SerializeField] private byte _modelType;

        private void Start()
        {
            using (MemoryStream buffer = new())
            {
                buffer.WriteByte((byte) ServerCommand.CREATE_UNIT);

                buffer.WriteByte((byte)_spellIds.Count);
                foreach (int id in _spellIds)
                {
                    buffer.Write(BitConverter.GetBytes(id), 0, sizeof(int));
                }

                StatsTable stats = StatsTable.UNIT_DEFAULT;

                stats[UnitStat.ATK] = new PercentModifiedValue(100, 100);
                stats[UnitStat.MAX_HEALTH] = new PercentModifiedValue(1000, 100);
                stats[UnitStat.SPEED] = new PercentModifiedValue(3, 100);
                stats[UnitStat.HASTE] = new PercentModifiedValue(400, 100);

                for (int i = 0; i < StatsTable.STATS_COUNT; i++)
                {
                    buffer.Write(BitConverter.GetBytes(_stats[i].BaseValue), 0, sizeof(float));
                    buffer.Write(BitConverter.GetBytes(_stats[i].Percent), 0, sizeof(float));
                }

                buffer.Write(BitConverter.GetBytes(_leftResourceMax), 0, sizeof(float));
                buffer.Write(BitConverter.GetBytes(_rightResourceMax), 0, sizeof(float));
                buffer.Write(BitConverter.GetBytes((ushort) _leftType), 0, sizeof(ushort));
                buffer.Write(BitConverter.GetBytes((ushort) _rightType), 0, sizeof(ushort));


                buffer.Write(BitConverter.GetBytes(_startPostion.x), 0, sizeof(float));
                buffer.Write(BitConverter.GetBytes(_startPostion.y), 0, sizeof(float));
                buffer.Write(BitConverter.GetBytes(_startPostion.z), 0, sizeof(float));
                buffer.Write(BitConverter.GetBytes(_rotation), 0, sizeof(float));

                buffer.WriteByte((byte) _team);

                buffer.Write(BitConverter.GetBytes(_id), 0, sizeof(int));

                buffer.WriteByte(0);

                ServerTemplate.HandleData(buffer.ToArray());
            }
        }
    }
}
