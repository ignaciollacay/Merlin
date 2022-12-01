using System;
using System.Collections;
using UnityEngine;


/// <summary>
/// Level Manager WIP
/// Currently untested scripted behaviour for changing Dojo Scene.
/// </summary>
[DefaultExecutionOrder(-1000)] // Requires AssessmentManager to be loaded, but LevelManager.Start() needs to execute before AssessmentManager.Start();
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelSOList levels;

    private SceneHandlerManager SceneManager => SceneHandlerManager.Instance;
    private AssessmentManager AssessmentManager => AssessmentManager.Instance;
    private DialogueManager DialogueManager => DialogueManager.Instance;
    private Dialogue PetDialogue => DialogueManager.Instance.petDialogue.dialogue;

    // Starting Scene Management
    public override void Awake()
    {
        base.Awake(); // Singleton Instance
    }
    private void Start()
    {
        LevelSO currentLevelSO = levels.GetCurrentLevel();
        Debug.Log("CurrentLevelSO=" + currentLevelSO + "\n CurrentLevel = " + levels.currentLevel);
        SetScene(currentLevelSO);
    }

    private void SetScene(LevelSO level)
    {
        SetDialogues(level);
        SetSceneHandler(level);
        SetAssignment(level);
    }

    private void SetDialogues(LevelSO level)
    {
        DialogueManager.Instance.SetPetDialogue(level.startDialogue);
    }

    private void SetAssignment(LevelSO level)
    {
        AssessmentManager.Instance.SetAssignment(level.assignedSpells);
    }

    private void SetSceneHandler(LevelSO level)
    {
        SceneHandlerManager.Instance.SetScene(level.nextScene);
    }

    // Next Scene Management.
    // TODO: Execute from On Assignment End 
    public void SetNextLevel()
    {
        levels.SetNextLevel();
    }

    // TODO: Run when starting a new game on Landing Page.
    public void ResetLevelCount()
    {
        levels.ResetLevelCount();
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        ResetLevelCount();
    }
#endif
}