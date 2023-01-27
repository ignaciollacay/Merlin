using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class VoskDebug : MonoBehaviour
{
    [SerializeField] private VoskSpeechToText voskSpeechToText;
    public List<TextMeshProUGUI> _debugTexts = new List<TextMeshProUGUI>();

    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private GameObject pfText;
    [SerializeField] private RectTransform layout;
    private float textHeight;

    private void Awake()
    {
        textHeight = pfText.GetComponent<RectTransform>().sizeDelta.y;
        dropdown.value = voskSpeechToText.MaxAlternatives + 1;
        SetNewAlternativeCount(voskSpeechToText.MaxAlternatives);
    }
    public void SetNewAlternativeCount(int count)
    {
        if (_debugTexts.Count < count)
        {
            var obj = Instantiate(pfText, layout);
            var text = obj.GetComponent<TextMeshProUGUI>();
            _debugTexts.Add(text);
            Vector2 newLayoutSize = new Vector2(layout.sizeDelta.x, layout.sizeDelta.y + textHeight);
            layout.sizeDelta = newLayoutSize;
        }
        else if (_debugTexts.Count == count)
        {
            return;
        }
        else
        {
            for (int i = 0; i < _debugTexts.Count; i++)
            {
                if (i > count)
                {
                    _debugTexts.Remove(_debugTexts[i]);
                    Vector2 newLayoutSize = new Vector2(layout.sizeDelta.x, layout.sizeDelta.y - textHeight);
                    layout.sizeDelta = newLayoutSize;
                }
            }
        }
        SetNewAlternativeCount(count);
    }
}