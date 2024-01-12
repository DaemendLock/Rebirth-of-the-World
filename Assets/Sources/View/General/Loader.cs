using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace View.General
{
    public class Loader : MonoBehaviour
    {
        private static Loader _instance;

        [SerializeField] private Image _fillImage;

        private void Start()
        {
            gameObject.SetActive(false);

            if (_instance != null)
            {
                return;
            }

            DontDestroyOnLoad(gameObject);
            _instance = this;
        }

        public static Task LoadScene(int sceneId)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

            if (_instance == null)
            {
                return Task.CompletedTask;
            }

            return Task.Run(() => _instance.ShowProgress(operation));
        }

        private void ShowProgress(AsyncOperation operation)
        {
            _instance.gameObject.SetActive(true);

            while (operation.isDone == false)
            {
                _fillImage.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
            }

            gameObject.SetActive(false);
        }
    }
}