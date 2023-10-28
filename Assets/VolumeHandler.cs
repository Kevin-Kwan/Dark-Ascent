/*
 * File: VolumeHandler.cs
 * Authors: Kevin Kwan
 * Created: 10/27/2023
 * Modified: 10/27/2023
 * Description: This script handles the volume slider in the settings menu.
 * The volume slider is used to change the volume of the game.
 * The volume is stored in PlayerPrefs to be loaded whenever the player continues the game.
 * Thus, volume is persistent across scenes.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour
{
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        float volume = PlayerPrefs.HasKey("volume") ? PlayerPrefs.GetFloat("volume") : 0.5f;
        AudioListener.volume = volume;
        volumeSlider.value = volume;
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
}