using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaHitbox : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}