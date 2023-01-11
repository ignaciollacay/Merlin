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
    public int overrideLevel = 0;

    private SceneHandlerManager SceneManager => SceneHandlerManager.Instance;
    private AssessmentManager AssessmentManager => AssessmentManager.Instance;
    private DialogueManager DialogueManager => DialogueManager.Instance;
    private Dialogue PetDialogue => DialogueManager.Instance.petDialogue.dialogue;

    // Starting Scene Management
    public override void Awake()
    {
        base.Awake(); // Singleton Instance

        if (overrideLevel != 0)
        {
            levels.SetLevel(overrideLevel);
        }
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

        // TODO: Should Refactor
        if (level.scene.sceneName == "Dojo")
        {
            SetAssignment(level);
        }
        if (level.scene.sceneName == "Battle")
        {
            SetOpponent(level);
        }
    }

    private void SetDialogues(LevelSO level)
    {
        DialogueManager.Instance.SetPetStartDialogue(level.startDialogue);
        DialogueManager.Instance.SetEndDialogue(level.endDialogue);
    }

    private void SetSceneHandler(LevelSO level)
    {
        SceneHandlerManager.Instance.SetScene(level.nextScene);
    }

    private void SetAssignment(LevelSO level)
    {
        AssessmentManager.Instance.SetAssignment(level.assignedSpells);
    }

    private void SetOpponent(LevelSO level)
    {
        BattleHandler.Instance.SetEnemies(level.assignedEnemies);
    }

    // Executed from Unity Event in Scene (Assessment or Battle Manager) 
    public void SetNextLevel()
    {
        levels.SetNextLevel();
    }

    // TODO: Run when starting a new game on Landing Page.
    public void ResetLevelCount()
    {
        levels.ResetLevelCount();
    }

    public void SetLevel(int levelCount)
    {
        levels.SetLevel(levelCount);
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        ResetLevelCount();
    }
#endif
}