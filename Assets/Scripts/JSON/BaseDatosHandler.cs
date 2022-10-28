using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class BaseDatosHandler : MonoBehaviour
{
    //public BaseDatosObjeto bd;

    [MenuItem("Tools/Update ScriptableObject/Dialogues")]
    private static void CreateSOFromJSON()
    {
        string datos = File.ReadAllText(Application.dataPath + "/Resources/Database - Dialogues.json");
        BaseDatosObjeto bd = AssetDatabase.LoadAssetAtPath<BaseDatosObjeto>("Assets/Resources/Dialogues.asset");
        JsonUtility.FromJsonOverwrite(datos, bd);
        
        foreach (var dialogo in bd.Dialogues)
        {
            dialogo.Lines = dialogo.Lines.Where(s => !string.IsNullOrWhiteSpace(s)).ToList(); // remove empty dialogue lines
        }
    }
    //TextAsset dbJson;

    [MenuItem("Tools/Update JSON/Dialogues")]
    private static void CreateJSONFromSO()
    {
        // Load SO with new JSON data
        BaseDatosObjeto bd = AssetDatabase.LoadAssetAtPath<BaseDatosObjeto>("Assets/Resources/Dialogues.asset");

        // Create JSON String with new data
        string datos = JsonUtility.ToJson(bd);

        File.WriteAllText("Assets/Resources/dialogosSOToJSON.json", datos); 


        // Create object with Json string
        //TextAsset json = new TextAsset(datos);

        //AssetDatabase.CreateAsset(json, "Assets/Resources/dialogosSOToJSON.asset");
        //AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
