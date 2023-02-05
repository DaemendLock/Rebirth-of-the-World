using System.Collections.Generic;
using UnityEngine;

public class ScenarioSelection : MonoBehaviour
{
    public List<GameObject> scnarioList = new List<GameObject>();
    private GameObject currentUse;

    private void Start() {
        currentUse = scnarioList[0];
    }

    public void OpenScenarioList(int num) {
        if (num<0||num>= scnarioList.Count) {
            return;
        }
        currentUse.SetActive(false);
        scnarioList[num].SetActive(true);
        currentUse = scnarioList[num];
    }

    public void OpenScenarioByCursor() {
        OpenScenarioList(6 - (int)(Input.mousePosition.y - transform.position.y - 20)/120);
    }
}
