/*
 * File: SpikeHazard.cs
 * Author: Amal Chaudry
 * Created: 10/22/2023
 * Modified: 10/28/2023
 * Description: This script causes player death upon jumping on spikes.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHazard : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("3rdPPlayer");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            Debug.Log("spikes hit");
            ThirdPController playerController = player.GetComponent<ThirdPController>();
            if (playerController != null) {
                playerController.health = 0;
                Debug.Log("dead");
            }
        }
    }
}
