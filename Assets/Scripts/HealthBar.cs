using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    [SerializeField] private bool useGradient = true;

    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        UpdateHealth(health);
    }
    public void UpdateHealth(int health)
    {
        healthBar.value = health;

        if (useGradient)
        {
            fill.color = gradient.Evaluate(healthBar.normalizedValue);
        }
    }
}
