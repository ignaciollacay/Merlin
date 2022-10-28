using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
[CreateAssetMenu(fileName = "New database", menuName = "so/database")]
public class BaseDatosObjeto : ScriptableObject
{
    public List<Dialogo> Dialogues;
}


