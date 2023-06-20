using System;
using System.IO;

namespace Data {
    public static class DataLoader {
        private const string VERSION_FILE_PATH = "StreamingAssets/version.txt";
        private const string REFERENCE_FILE_PATH = "StreamingAssets/References";

        public static void LoadAllUnitData() {

        }

        public static void LoadAllItemData() {
            //new UnitData();
        }

        public static long ReadVersionData() => long.Parse(File.ReadAllText(VERSION_FILE_PATH));
         
        

    }

    public interface ILoadableData {
        void Save(string path);
        ILoadableData Load(string path, string name);
    }

    [Serializable]
    public class UnitDataFile :ILoadableData {
        private OldUnitData _data;

        public ILoadableData Load(string path, string name) {
            throw new NotImplementedException();
        }

        public void Save(string path) {
            throw new NotImplementedException();
        }
    }


}