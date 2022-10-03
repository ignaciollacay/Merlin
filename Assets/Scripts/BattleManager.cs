using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// proviorio para dar fin al primer loop
//
public class BattleManager : MonoBehaviour
{
    [SerializeField] SpellBook spellBook;
    [SerializeField] private Canvas foxCanvas;
    [SerializeField] private Text messageText;
    [SerializeField] private Text helpText;
    [SerializeField] private string messageString = "Un planeta necesita tu ayuda para defenderse de enemigos. Es hora de probar tus hechizos en combate";
    [SerializeField] private string helpString = "Presiona para ir al combate";

    private void Update()
    {
        if (spellBook.discoveredSpells.Count == 2)
        {
            StartBattle();
        }
    }

    async void StartBattle()
    {
        await Task.Delay(8000);
        messageText.text = messageString;
        helpText.text = helpString;
        foxCanvas.enabled = true;
    }
}
