using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Dialogue", menuName = "SO/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public double id;
    public string Name;
    public string character;
    [TextArea(5, 10)]
    public string description = "";
    [TextArea (3,10)] public List<string> sentences;
}
