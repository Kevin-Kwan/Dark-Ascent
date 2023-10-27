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