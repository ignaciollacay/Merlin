using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    // Make a list?
    public SceneSO sceneSO;

    public void LoadAsync()
    {
        SceneManager.LoadSceneAsync(sceneSO.sceneName);
    }

    public void SetScene(SceneSO scene)
    {
        sceneSO = scene;
    }

    public static void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
