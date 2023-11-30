using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    private Color fullHealthColor; // Store the full health color

    private void Start()
    {
        fullHealthColor = new Color(0f, 1f, 0f, 1f); // Store the original color when the health bar initializes
    }

    public void SetMaxHealth(float health) {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health) {
        slider.value = health;
        if (health <= 100) {
            fill.color = fullHealthColor;
        }
        if (health <= 50) {
            fill.color = new Color(1f, 0.92f, 0.016f, 1f);
        }
        if (health <= 20) {
            fill.color = new Color(1f, 0f, 0f, 1f); 
        }
    }
}
