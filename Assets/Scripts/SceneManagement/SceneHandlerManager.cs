using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandlerManager : Singleton<SceneHandlerManager>
{
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
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void LoadAfterDialogueEnd(DialogueHandler dialogueManager)
    {
        dialogueManager.DialogueEnd.AddListener(LoadAsync);
    }
}
