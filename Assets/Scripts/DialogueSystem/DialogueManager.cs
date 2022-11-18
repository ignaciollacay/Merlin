using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DefaultExecutionOrder(-500)]
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Text dialogue;
    [Tooltip("Dialogue Button to add DisplayNextSentence On Click")] // TODO: Is there only one dialogue button? Maybe make list? Or join canvases?
    [SerializeField] public Button button; // Button is actually in box GO, could use GetComponent. But makes code less reusable.

    private Queue<string> sentences = new Queue<string>();

    public UnityEvent DialogueEnd;

    public static DialogueManager Instance;

    public void Awake()
    {
        Instance = this;
        //button.onClick.AddListener(DisplayNextSentence); 
        //FIXME: Disabling line obove
        //Listener Added On Scene... Why is it duplicated
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        //sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    /// <summary>
    /// Run from button on click event. Shows the next line if sentences remain, else runs end dialogue.
    /// </summary>
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogue.text = sentence;
    }

    private void EndDialogue() => DialogueEnd?.Invoke();
}
