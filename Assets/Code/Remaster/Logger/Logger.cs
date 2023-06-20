using Remaster.Events;
using Remaster.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Remaster
{
    public static class Logger
    {
        private static List<string> _datas = new List<string>();

        public static void LogCastSucceed(EventData data)
        {
            Add($"{data.Caster} -> {data.Target}: casted {data.Spell.Id}");
        }

        public static void LogCastFailed(EventData data, CommandResult result)
        {
            Add($"Spell({data.Spell.Id}) cast failed({result}): {data.Caster} -> {data.Target}");
        }

        public static void Log(string logText)
        {
            Add(logText);
        }

        public static void SaveLog(string path)
        {
            FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);

            using (StreamWriter file = new StreamWriter(stream))
            {
                foreach (string data in _datas)
                {
                    file.WriteLine(data);
                }
            }

            stream.Close();
        }

        public static  void ShowLatestLogs(int count)
        {
            int endAt = Math.Max(0, _datas.Count - count);

            for (int i = _datas.Count - 1; i >= endAt; i++)
            {
                Debug.Log(_datas[i]);
            }
        }

        private static void Add(string data)
        {
            lock (_datas)
            {
                _datas.Add(data);
            }
        }
    }
}
