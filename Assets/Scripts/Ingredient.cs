using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour
{
    public ItemSO item;
    public GameObject itemText;

    private Text text;
    private PhraseRecognition phraseRecognition;

    private void Awake()
    {
        if (itemText.TryGetComponent<Text>(out text))
        {
            text.text = item.name;
        }
        else
        {
            Debug.Log("Missing text component in " + itemText);
        }

        if (itemText.TryGetComponent<PhraseRecognition>(out phraseRecognition))
        {
            phraseRecognition.readPhrase = item.name;
        }
        else
        {
            Debug.Log("Missing Phrase Recognition component in " + itemText);
        }
    }

    private void Start()
    {
        phraseRecognition.OnPhraseRecognized += SelectItem;
    }

    void SelectItem()
    {
        CraftManager.Instance.ItemsSelected(item);
    }
}
