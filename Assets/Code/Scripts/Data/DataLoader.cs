using System;

namespace Data {
    public static class DataLoader {


        public static void LoadAllUnitData() {

        }

        public static void LoadAllItemData() {

            //new UnitData();


        }
    }

    public interface ILoadableData {
        void Save(string path);
        ILoadableData Load(string path, string name);
    }

    [Serializable]
    public class UnitDataFile :ILoadableData {
        private UnitData _data;

        public ILoadableData Load(string path, string name) {
            throw new NotImplementedException();
        }

        public void Save(string path) {
            throw new NotImplementedException();
        }
    }


}