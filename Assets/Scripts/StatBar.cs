using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Slider))]
public class StatBar : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private Slider slider;

    [SerializeField] private bool useGradient = false;

    public void SetStatMax(int stat)
    {
        slider.maxValue = stat;
        UpdateStat(stat);
        
    }

    public void UpdateStat(int stat)
    {
        slider.value = stat;

        if (useGradient && gradient != null)
        {
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}