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
}
