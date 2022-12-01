using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO_new", menuName = "SO/Level")]
public class LevelSO : ScriptableObject
{
    [Header("Scene Properties")]
    //public int levelCount; // TODO: get set? Remove? Defined by LevelSO List.
    public SceneSO scene;

    [Header("Scene Variable References")]
    public InventorySpellSO assignedSpells;
    public DialogueSO startDialogue;
    public SceneSO nextScene; // TODO: get set? Defined by LevelSO List.


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