/*
 * File: CheckpointHandler.cs
 * Author: Connor Sugasawara
 * Created: 10/28/2023
 * Modified: 10/28/2023
 * Description: Collider Checking for Audio
 * 
 * Contributions:
 *   Connor Sugasawara:
 *    - Basic Functionality
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointReached : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // play audio once
            audioSource.Play();
            // disable script so only one cue per scene
            this.enabled = false;
        }
    }
}
