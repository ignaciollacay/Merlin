using UnityEditor;
using UnityEngine;

public class CreateSOfromJSON : MonoBehaviour
{   
    //[MenuItem("Tools/CreateDialogueFromJSON")]
    public static void CreateDialogue()
    {
        TextAsset db = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/ScriptableObjects/db_Dialogues.json");

        DialogueSO dialogue = ScriptableObject.CreateInstance<DialogueSO>();
        dialogue = JsonUtility.FromJson<DialogueSO>(db.text);
        AssetDatabase.CreateAsset(dialogue, $"Assets/ScriptableObjects/myDialogues/{dialogue.Name}.asset");

        /*
        // FIXME: can't use commas in dialogue.
        string[] jsons = db.text.Split(';');

        foreach (string json in jsons)
        {
            Debug.Log(json);
            DialogueSO dialogue = ScriptableObject.CreateInstance<DialogueSO>();
            string dialogue = JsonUtility.FromJson(json); // FIXME: Error. Cannot deserialize JSON to new instances of type 'DialogueSO.'
            AssetDatabase.CreateAsset(dialogue, $"Assets/ScriptableObjects/Dialogues/{dialogue.Name}.asset");
        }
        */
        AssetDatabase.SaveAssets(); // save assets to disk
        AssetDatabase.Refresh(); // refresh editor
    }
}
