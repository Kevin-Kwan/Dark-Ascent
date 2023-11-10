using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollectable : MonoBehaviour
{
    public float rangeIncrease = 1f;  // Adjust as needed
    public float intensityIncrease = 0.5f;  // Adjust as needed
    public AudioSource audioSource;
    public float hoverAmplitude = 0.5f;  // The height the object will hover
    public float hoverFrequency = 1f;    // The speed of the hover
    private Vector3 initialPosition;

    void Start()
    {
        //initialPosition = transform.position;
    }

    void Update()
    {
        Hover();
    }

    void Hover()
    {
        // float yOffset = hoverAmplitude * Mathf.Sin(hoverFrequency * Time.time);
        // transform.position += new Vector3(0, yOffset, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Light playerLight = other.GetComponentInChildren<Light>();
            if (playerLight != null)
            {
                audioSource.Play();
                playerLight.range += rangeIncrease;
                playerLight.intensity += intensityIncrease;
                Destroy(gameObject);  // Destroy the collectible
            }
        }
    }
}
