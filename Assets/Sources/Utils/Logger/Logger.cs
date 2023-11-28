using System.Collections.Generic;
using System.IO;

namespace Utils.Logger
{
    public static class Logger
    {
        private static List<LoggerData> _datas = new List<LoggerData>();

        public static void Log(LoggerData data)
        {
            lock (_datas)
            {
                _datas.Add(data);
            }
        }

        public static void SaveLog(string path)
        {
            FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);

            using (StreamWriter file = new StreamWriter(stream))
            {
                foreach (LoggerData data in _datas)
                {
                    file.WriteLine(data.ParseLog());
                }
            }

            stream.Close();
        }
    }
}
