using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;
    
    public void UpdateHealthBar(float value)
    {
        healthBar.value = value;
    }
}
