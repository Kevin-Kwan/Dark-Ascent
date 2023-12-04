using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DemonHealth : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    private Color fullHealthColor; // Store the full health color

    private void Start()
    {
        slider.gameObject.SetActive(false);
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Level7") {
            slider.gameObject.SetActive(true);
        }
        fullHealthColor = new Color(1f, 0f, 0f, 1f); // Store the original color when the health bar initializes
    }

    public void SetMaxHealth(float health) {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health) {
        slider.value = health;
    }
}
