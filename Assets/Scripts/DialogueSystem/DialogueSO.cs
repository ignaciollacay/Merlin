using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "SO/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string character;
    public double id;
    [TextArea (3,10)] public string[] sentences;
}
