/*
 * File: VolumeHandler.cs
 * Authors: Kevin Kwan, Connor Sugasawara
 * Created: 10/27/2023
 * Modified: 10/28/2023
 * Description: This script handles the volume sliders in the settings menu.
 * The volume sliders are used to change the master volume, music volume, and sound effect volume.
 * The volume is stored in PlayerPrefs to be loaded whenever the player continues the game.
 * Thus, volume is persistent across scenes.
 * 
 * Contributions:
 *   Kevin Kwan:
 *   - Created global volume control slider
 *   - Saved volume level in player preferences
 *   Connor Sugasawara:
 *   - Split implementation into master volume, music volume, and sound effect volume sliders
 *   - Added listeners for slider changes
 *   - Reworked code to use AudioMixer
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeHandler : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioMixer mixer;
    public float multiplier = 30f;

    [SerializeField] string masterVolumeParameter = "MasterVolume";
    [SerializeField] string musicVolumeParameter = "MusicVolume";
    [SerializeField] string sfxVolumeParameter = "SFXVolume";

    // Start is called before the first frame update
    void Start()
    {
        masterSlider.value = PlayerPrefs.HasKey(masterVolumeParameter) ? PlayerPrefs.GetFloat(masterVolumeParameter) : 0.8f;
        musicSlider.value = PlayerPrefs.HasKey(musicVolumeParameter) ? PlayerPrefs.GetFloat(musicVolumeParameter) : 1f;
        sfxSlider.value = PlayerPrefs.HasKey(sfxVolumeParameter) ? PlayerPrefs.GetFloat(sfxVolumeParameter) : 1f;
        masterSlider.onValueChanged.AddListener(MasterSliderValueChanged);
        musicSlider.onValueChanged.AddListener(MusicSliderValueChanged);
        sfxSlider.onValueChanged.AddListener(SFXSliderValueChanged);
    }

    private void MasterSliderValueChanged(float value)
    {
        mixer.SetFloat(masterVolumeParameter, Mathf.Log10(value) * multiplier);
        PlayerPrefs.SetFloat(masterVolumeParameter, value);
    }

    private void MusicSliderValueChanged(float value)
    {
        mixer.SetFloat(musicVolumeParameter, Mathf.Log10(value) * multiplier);
        PlayerPrefs.SetFloat(musicVolumeParameter, value);
    }

    private void SFXSliderValueChanged(float value)
    {
        mixer.SetFloat(sfxVolumeParameter, Mathf.Log10(value) * multiplier);
        PlayerPrefs.SetFloat(sfxVolumeParameter, value);
    }
}