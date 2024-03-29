﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "LevelSOList_new", menuName = "SO/Inventory/Level List")]
public class LevelSOList : ScriptableObject
{
    public List<LevelSO> levels;
    public int currentLevel; // TODO: Made public for testing. Set to private

    // FIXME: Buggy. Doesn't allow to recicle the same Level Asset in different Lists since next scene will change according to the list it was last validated in.
    //private void OnValidate()
    //{
    //    foreach (LevelSO level in levels)
    //    {
    //        if (level != null)
    //            level.nextScene = GetNextScene();
    //    }
    //}

    public LevelSO GetCurrentLevel()
    {
        return levels[currentLevel];
    }

    private SceneSO GetNextScene()
    {
        int nextLevelCount = currentLevel + 1;
        if (nextLevelCount >= levels.Count)
        {
            nextLevelCount = 0;
        }
        LevelSO nextLevel = levels[nextLevelCount];
        SceneSO nextScene = nextLevel.scene;

        return nextScene;
    }

    public void SetNextLevel()
    {
        currentLevel++;
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }

    public void ResetLevelCount()
    {
        currentLevel = 0;
    }
}
