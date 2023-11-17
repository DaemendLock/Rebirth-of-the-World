using System.Collections.Generic;
using UnityEngine;

namespace Data.Model
{
    public static class ModelLibrary
    {
        private static readonly Dictionary<string, string> _modelPathes = new();

        private static readonly Dictionary<string, GameObject> _models = new();

        public static GameObject LoadModel(string modelName)
        {
            return Resources.Load<GameObject>(_modelPathes[modelName]);
        }

        public static void LoadModel(string modelName, GameObject model)
        {
            _models[modelName] = model;
        }

        public static void Free()
        {
            _models.Clear();
        }

        public static GameObject GetModel(string modelName)
        {
            return _models[modelName];
        }
    }
}
