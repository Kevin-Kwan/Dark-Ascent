/*
 * File: LavaHitbox.cs
 * Author: Kevin Kwan
 * Created: 10/20/2023
 * Modified: 10/20/2023
 * Description: This script adds functionality to the lava pool/void:
 *   - If the player touches the lava, they die.
 *   - If a pushable object touches the lava, it respawns back to its original position.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaHitbox : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("3rdPPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.gameObject == player)
        {
            ThirdPController playerController = player.GetComponent<ThirdPController>();
            if (playerController != null)
            {
                playerController.health = 0;
                Debug.Log("Player entered lava, health set to 0");
            }
            else
            {
                Debug.Log("ThirdPController component not found on player");
            }
        }
        if (other.gameObject.CompareTag("Pushable"))
        {
            Pushable pushable = other.gameObject.GetComponent<Pushable>();
            if (pushable != null)
            {
                pushable.Respawn();
                Debug.Log("Pushable entered lava, respawned");
            }
            else
            {
                Debug.Log("Pushable component not found on pushable object");
            }
        }
    }
}