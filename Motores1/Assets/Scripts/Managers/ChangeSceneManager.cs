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

        //1 Es la escena de lvl 1
        if (sceneToLoad < 1)
            sceneToLoad = 1;
        if(sceneToLoad > SceneManager.sceneCountInBuildSettings)
            sceneToLoad = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnDestroy()
    {
        GameManager.Instance.ChangeScenManager = null;
    }
}
