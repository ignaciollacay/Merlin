using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialBar : MonoBehaviour
{
    private Image ring;
    public bool cooldownBool = false;
    public float cooldownTime;

    float currentTime = 0f;

    private void Awake()
    {
        ring = GetComponent<Image>();
    }

    private void Update()
    {
        if (cooldownBool)
        {
            if (currentTime <= cooldownTime)
            {
                currentTime += Time.deltaTime;
                ring.fillAmount = Mathf.Lerp(1, 0, currentTime / cooldownTime);
            }
            else
            {
                ring.fillAmount = 0;
                currentTime = 0f;
                cooldownBool = false;
            }
        }
       
    }

}
