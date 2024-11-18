using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.ChangeScenManager = this;
    }

    public void CallSceneChange(int num)
    {
        var sceneToLoad = num;
        Debug.Log($"{sceneToLoad}, {num}, {SceneManager.sceneCountInBuildSettings}");

        //1 Es la escena de lvl 1
        if (sceneToLoad < 1)
            sceneToLoad = 1;
        if(sceneToLoad > SceneManager.sceneCountInBuildSettings)
            sceneToLoad = SceneManager.sceneCountInBuildSettings - 1;
        Debug.Log($"{sceneToLoad}, {num}, {SceneManager.sceneCountInBuildSettings}");
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnDestroy()
    {
        GameManager.Instance.ChangeScenManager = null;
    }
}
