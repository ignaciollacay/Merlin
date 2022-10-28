using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

// FIXME: Distinguish between commas in values & comma separator
// Passes values from CSV to Scriptable Objects
public class CSVParser
{
    // Spell CSV directory path
    private static string SpellCSVPath = "Assets/ScriptableObjects/database - Spells.csv";
    // Spell SO directory path
    private static string SpellsPath = "Assets/ScriptableObjects/Spells/";
    // Spell CSV directory path
    private static string DialogueCSVPath = "Assets/ScriptableObjects/database - Dialogues.csv";
    // Dialogue SO directory path
    private static string DialoguesPath = "Assets/ScriptableObjects/Dialogues/";

    [MenuItem("Tools/Scriptable Objects/From CSV/Create Spells")]
    public static void UpdateSpells()
    {
        TextAsset csv = AssetDatabase.LoadAssetAtPath<TextAsset>(SpellCSVPath);
        CreateSO(ConvertCSV(csv), SOtype.Spell);
    }

    [MenuItem("Tools/Scriptable Objects/From CSV/Create Dialogues")]
    public static void UpdateDialogues()
    {
        TextAsset csv = AssetDatabase.LoadAssetAtPath<TextAsset>(DialogueCSVPath);
        //CreateDialogueSOs(ParseCSV(csv));
        CreateSO(ConvertCSV(csv), SOtype.Dialogue);
    }

    // Returns CSV as a dictionary where Tkey : row/line number & TValue : columns
    public static Dictionary<int, string[]> ConvertCSV(TextAsset csv)
    {
        // dictionary containing Row as Keys & Columns as Value
        Dictionary<int, string[]> table = new Dictionary<int, string[]>();

        string[] rows = csv.text.Split('\n'); // row separator
        rows = rows.Skip(1).ToArray(); // remove header

        for (int i = 0; i < rows.Count(); i++)
        {
            // FIXME: can't use commas in dialogue.
            string[] columns = rows[i].Split(','); // column separator  
            table.Add(i, columns);
        }

        return table;
    }

    public static void CreateSO(Dictionary<int, string[]> table, SOtype SO)
    {
        foreach (var row in table.Keys)
        {
            string[] columns = table.GetValueOrDefault(row);

            switch (SO)
            {
                case SOtype.Spell:
                    CreateSpellSOs(row, columns);
                    break;
                case SOtype.Dialogue:
                    CreateDialogueSOs(row, columns);
                    break;
            }
        }

        AssetDatabase.SaveAssets(); // save assets to disk
        AssetDatabase.Refresh(); // refresh editor
    }

    public static void CreateSpellSOs(int row, string[] columns)
    {
        SpellSO spell = ScriptableObject.CreateInstance<SpellSO>();

        SetSpellValues(spell, row, columns[0], columns[1], columns[2], columns[3], columns[4], columns[5]);

        AssetDatabase.CreateAsset(spell, $"{SpellsPath}{spell.Name}.asset");
    }
    public static void CreateDialogueSOs(int row, string[] columns)
    {
        DialogueSO dialogue = ScriptableObject.CreateInstance<DialogueSO>();

        IEnumerable<string> sentences = columns.Skip(3); //skip non-dialogue lines
        sentences = sentences.Where(s => !string.IsNullOrWhiteSpace(s)); // remove empty dialogue lines
        SetDialogueValues(dialogue, row, columns[0], columns[1], columns[2], sentences); // set values

        AssetDatabase.CreateAsset(dialogue, $"{DialoguesPath}{dialogue.Name}.asset");
    }

    public static void SetSpellValues(SpellSO spell, int id, string name, string phrase, string type, string value, string cooldown, string description)
    {
        spell.id = id;
        spell.Name = name;
        spell.phrase = phrase;
        spell.type = System.Enum.Parse<SpellType>(type);
        spell.value = int.Parse(value);
        spell.cooldown = int.Parse(cooldown);
        spell.description = description;
    }
    public static void SetDialogueValues(DialogueSO dialogue, int id, string name, string character, string description, IEnumerable<string> sentences)
    {
        dialogue.id = id;
        dialogue.Name = name;
        dialogue.character = character;
        dialogue.description = description;
        dialogue.sentences = sentences.ToList();
    }
}


/*
    public class CSVtoSO
{
    // CSV directory path
    private static string CSVpath = "/Resources/databaseSpells.csv";
    // SpellSO directory path
    private static string spellSOsPath = "/ScriptableObjects/Spells";

    // TODO: Generic ParseCSV for different types of variables to use for both Spells & Dialogs... Method could be non-static to recieve corresponding CSV, & values could be stored in a dictionary to be assigned in static methods UpdateSpells, UpdateDialogs, instead of assigning them in here
    public static void UpdateSpells()
    {
        // Load spell csv Asset
        TextAsset csv = Resources.Load<TextAsset>(Application.dataPath + CSVpath);
        // Load Spell Scriptable Objects
        SpellSO[] spells = Resources.LoadAll(Application.dataPath + spellSOsPath, typeof(SpellSO)).Cast<SpellSO>().ToArray();

        // Separate Rows
        string[] rows = csv.text.Split('\n');

        // for each row
        for (int i = 0; i < rows.Length; i++)
        {
            // Skip first line (contains csv header)
            if (i == 0)
                continue;

            // Separate each row into columns
            string[] columns = rows[i].Split(",");

            int id = int.Parse(columns[0]);
            string name = columns[1];
            string phrase = columns[2];
            SpellcastType type = System.Enum.Parse<SpellcastType>(columns[3]);
            int value = int.Parse(columns[4]);
            int cooldown = int.Parse(columns[5]);
            string description = columns[6];

            var spell = FindSO(spells, columns);
            if (spell != null)
            {
                // Assign values
                AssignValues2(spell, id, name, phrase, type, value, cooldown, description);
                // flag SO as dirty
                EditorUtility.SetDirty(spell);
            }
            else
            {
                // Didnt find Spell SO in project. Create new asset before assigning
                SpellSO newSpell = ScriptableObject.CreateInstance<SpellSO>();
                AssignValues2(newSpell, id, name, phrase, type, value, cooldown, description);
                AssetDatabase.CreateAsset(newSpell, Application.dataPath + spellSOsPath + "/" + spell.spellName);
            }
        }
    }
    static SpellSO FindSO(SpellSO[] spells, string[] columns)
    {
        foreach (var spell in spells)
        {
            if (spell.id.Equals(int.Parse(columns[0])))
            {
                return spell;
            }
        }
        return null;
    }

    public static void AssignValues2(SpellSO spell, int id, string name, string phrase, SpellcastType type, int value, int cooldown, string description)
    {

        spell.id = id;
        spell.spellName = name;
        spell.type = type;
        spell.value = value;
        spell.cooldown = cooldown;
        spell.description = description;
    }
}

// https://blog.floriancourgey.com/2021/04/unity-bulk-update-scriptable-object-csharp
public class UnityTools : MonoBehaviour
{
    // Spell CSV directory path
    private static string SpellCSVPath = "/Resources/databaseSpells.csv";
    // Spell SO directory path
    private static string SpellsSOPath = "/ScriptableObjects/Spells";

    [MenuItem("Tools/UpdateSpells")]
    public static void UpdateSpells()
    {
        TextAsset csv = Resources.Load<TextAsset>(Application.dataPath + SpellCSVPath);
        UpdateSpellSO(ParseCSV(csv));
    }

    public static Dictionary<int, string[]> ParseCSV(TextAsset csv)
    {
        // dictionary containing Row as Keys & Columns as Value
        Dictionary<int, string[]> table = new Dictionary<int, string[]>();

        string[] rows = csv.text.Split('\n'); // row separator
        rows = rows.Skip(1).ToArray(); // remove header

        for (int i = 0; i < rows.Count(); i++)
        {
            string[] columns = rows[i].Split(','); // column separator
            table.Add(i, columns);
        }

        return table;
    }

    public static void UpdateSpellSO(Dictionary<string, string[]> table)
    {
        // Load Spell Scriptable Objects
        SpellSO[] spells = Resources.LoadAll(Application.dataPath + SpellsSOPath, typeof(SpellSO)).Cast<SpellSO>().ToArray();

        string[] row = table.Keys.ToArray();

        // All the spells in the table
        for (int i = 0; i < row.Length; i++)
        {
            // Update existing spells in the project using id

            foreach (SpellSO spell in spells) // Could use dictionary & database instead of looping. But since it isn't on runtime...
            {
                if (row[i] == spell.id.ToString())
                {
                    string[] columns = table.GetValueOrDefault(row[i]);

                    SetSpellValues(spell, row[i], columns[0], columns[1], columns[2], columns[3], columns[4], columns[5]);

                    EditorUtility.SetDirty(spell);

                    // Stop searching for spells with matching id since it was already found
                    break;
                }
            }

            // Create new spells in project if they were not created

        }

        AssetDatabase.SaveAssets(); // save assets to disk
        AssetDatabase.Refresh(); // refresh editor
    }

    static SpellSO FindSO(SpellSO[] spells, string[] columns)
    {
        foreach (var spell in spells)
        {
            if (spell.id.Equals(int.Parse(columns[0])))
            {
                return spell;
            }
        }
        return null;
    }
    SpellSO FindSpellSO(SpellSO[] spells, Dictionary<string, string[]> table, out string id)
    {
        spell.id.Equals(table.ContainsKey
        string[] row = table.Keys.ToArray();

        for (int i = 0; i < row.Length; i++)
        {
            // Update existing spells in the project using id
            foreach (SpellSO spell in spells) // Could use dictionary & database instead of looping. But since it isn't on runtime...
            {
                if (row[i] == spell.id.ToString())
                {
                    id = row[i];
                    return spell;
                }
            }
        }
    }

    public static void SetSpellValues(SpellSO spell, string id, string name, string phrase, string type, string value, string cooldown, string description)
    {
        spell.id = int.Parse(id);
        spell.Name = name;
        spell.phrase = phrase;
        spell.type = System.Enum.Parse<SpellcastType>(type);
        spell.value = int.Parse(value);
        spell.cooldown = int.Parse(cooldown);
        spell.description = description;
    }




    public static void LoadSO()
    {
        SpellSO[] SO = Resources.LoadAll("myfolder2", typeof(SpellSO)).Cast<SpellSO>().ToArray(); // load all "Skill" Scriptable Objects
        foreach (SpellSO so in SO)
        {
            so.id = id; // update any field
            so.slots = new List<string> { "a", "b" }; // even List
            EditorUtility.SetDirty(so); // flag SO as dirty
        }
        Debug.Log("ComputeCharacters: saving db");
        AssetDatabase.SaveAssets(); // save assets to disk
        AssetDatabase.Refresh(); // refresh editor
        Debug.Log("ComputeCharacters: end");
    }

    


}

public class SpellSO : ScriptableObject
{
    public int id;
    public string Name;
    public string phrase;
    public SpellcastType type;
    public int value;
    public int cooldown;
    public string description;
}

// El problema con este script es que crea un nuevo SO.
// Yo quiero actualizar los existentes (load & save, instead of Create) 
    // Unity Tools si actualiza, pero no se bien como.
// Y quiero 
// https://www.youtube.com/watch?v=1EdLTF43d70
public class CSVtoSO2
{
    private static string enemyCSVPath = "/Editor/CSVs/EnemyCSV.csv";

    [MenuItem("Tools/GenerateEnemies")]
    public static void GenerateEnemies()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + enemyCSVPath);

        foreach (var s in allLines)
        {
            string[] splitData = s.Split(",");

            SpellSO spell = ScriptableObject.CreateInstance<SpellSO>();

            //pass the stats
            // TODO Compare CSV Header to variable for a more flexible approach
            // column 1
            spell.spellName = splitData[0];
            // column 2
            spell.Spell = splitData[1];
            // column 3
            // String needs to be converted into enum
            if (System.Enum.TryParse(splitData[2], true, out SpellcastType type))
                spell.type = type;
            // column 4
            // String needs to be converted into int
            spell.value = int.Parse(splitData[3]); // TODO: create value (generic amount for damage, defense, healing, etc.)
            // column 5
            // String needs to be converted into int
            spell.cooldown = int.Parse(splitData[4]);
            // column 6
            spell.description = splitData[5];

            AssetDatabase.CreateAsset(spell, "Assets/Enemies/" + spell);
        }

        AssetDatabase.SaveAssets();
    }
}
*/