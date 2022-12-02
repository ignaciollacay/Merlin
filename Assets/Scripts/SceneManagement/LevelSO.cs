using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO_new", menuName = "SO/Level")]
public class LevelSO : ScriptableObject
{
    [Header("Scene Properties")]
    public SceneSO scene;

    [Header("Scene Variable References")]
    public DialogueSO startDialogue;
    public DialogueSO endDialogue;
    public SceneSO nextScene; // TODO: Use get set instead. It is set by LevelSO List and should not be modified in Editor.

    [Header("Dojo Specific")]
    public InventorySpellSO assignedSpells;

    [Header("Battle Specific")]
    public EnemySOList assignedEnemies;



    public string GetLevelName()
    {
        return scene.sceneName;
    }

    //public InventoryDialogueSO inventoryDialogueSO;

    // Usaría un dialogo por cada objeto que use un dialogo Y que sea dinámico, que CAMBIE en cada REPETICION DEL NIVEL
    // Por ahora creo que solo tengo un pet dialogue. El resto son identicos en cada nivel?
    // Pero hay dos petDialogues creo. El de inicio y el de final.

    // Usaría un tipo de nivel para especificar como/donde van las referencias, llegado el caso que hay diferencias entre Battle y Dojo
    // Assessment Handler vs Battle Handler (Quizas de usar inheretance, ambos padres de un LevelHandler ?
    [TextArea(5, 10)]
    public string commentary;
}
