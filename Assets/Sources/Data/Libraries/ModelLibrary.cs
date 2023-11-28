using System.Collections.Generic;

namespace Data.Models
{
    public static class ModelLibrary
    {
        private static readonly Dictionary<int, Model> _models = new();

        public static void LoadModel(int id, Model model)
        {
            _models[id] = model;
        }

        public static void Free()
        {
            _models.Clear();
        }

        public static Model Get(int modelId)
        {
            return _models[modelId];
        }
    }
}
