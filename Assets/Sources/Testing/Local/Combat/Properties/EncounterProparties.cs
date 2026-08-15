using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Sources.Testing.Local.Combat.Properties
{
    public sealed class EncounterProperties : MonoBehaviour
    {
        //[Zenject.Inject] private readonly EncounterController _controller;

        [SerializeField] private SceneAsset _scene;

        private void Start()
        {
            SceneManager.LoadScene(_scene.name, LoadSceneMode.Additive);
        }
    }
}
