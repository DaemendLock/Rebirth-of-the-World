using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public static Loader Instance { get; private set; }

    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject loadingScene;

    private void Start()
    {
        gameObject.SetActive(false);

        if (Instance != null)
        {
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    //SceneSetup setup;

    /* public void LoadCombatScenario(Scenario scenario, List<UnitPreset> partySetup)
     {
         setup = new SceneSetup(scenario, partySetup);
         LoadScene(2);
     }*/

    public void LoadScene(int sceneId)
    {
        loadingScene.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    /*
    public void SetupScene(GameObject terrain, UnitPreset[] allies, UnitPreset[] enemy)
    {
        setup?.SetupGame(terrain, allies, enemy);
    }*/

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        while (!operation.isDone)
        {
            fillImage.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
        //if (setup != null)
        //{
        //    setup.SetupPartyUnits();
        //    setup = null;
        //}
        loadingScene.SetActive(false);
    }

    public void LeaveToMenu()
    {
        LoadScene(0);
    }

    public void LoadBufferedScenario()
    {
        //setup?.BufferedScenario.Load();
    }

    private void OnDestroy()
    {
        if (this != Instance)
        {
            return;
        }
        //Networking.ServerManager.Disonnect();
    }
}
