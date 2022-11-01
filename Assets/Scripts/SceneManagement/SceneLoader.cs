using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // TODO: Change to a Scriptable Object Class 
    public enum Scenes
    {
        LandingPage,
        Dojo,
        Battle
    }
    public static void Load(SceneSO sceneSO)
    {
        SceneManager.LoadScene(sceneSO.sceneName);
    }
}
