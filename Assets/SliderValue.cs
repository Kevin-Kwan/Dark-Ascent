/*
 * File: HeartUIController.cs
 * Author: Amal Chaudry
 * Created: 11/05/2023
 * Modified: 11/30/2023
 * Description: This script displays the numerical value of the volume sliders.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI sliderText;

    // Start is called before the first frame update
    void Start()
    {
        ShowSliderValue();
    }

    // Update is called once per frame
    void ShowSliderValue()
    {
         if (sliderText != null && slider != null) {
            int sliderInt = Mathf.RoundToInt(slider.value);
            sliderText.text = sliderInt.ToString();
        }
    }

    void Update() {
        ShowSliderValue();
    }
}
