using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public static void LoadScene(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        if (_instance == null)
        {
            return;
        }

        _instance.gameObject.SetActive(true);
        _instance.StartCoroutine(_instance.ShowProgress(operation));
    }

    private IEnumerator ShowProgress(AsyncOperation operation)
    {
        while (operation.isDone == false)
        {
            _fillImage.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
