/*
 * File: HeartUIController.cs
 * Author: Amal Chaudry
 * Created: 10/22/2023
 * Modified: 10/30/2023
 * Description: This script controls the heart Health UI. It removes hearts when the player's health decreases.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUIController : MonoBehaviour
{
    public GameObject player;
    private ThirdPController playerController;

    public Image[] heartsArray;
    public int maxHearts = 5;
    public int damagePerHeart = 20;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("3rdPPlayer");
        playerController = player.GetComponent<ThirdPController>();
        
        TakeAwayHeart();
    }

    private void TakeAwayHeart() {
        int displayHearts = (int) (playerController.health / damagePerHeart) - 1;

        for (int i = 0; i < heartsArray.Length; i++) {
            //Debug.Log("health " + playerController.health);
            Debug.Log("displayhearts " + displayHearts);
            if (playerController.health == 0) {
                for (int j = 0; j < heartsArray.Length; j++) {
                    heartsArray[j].enabled = false;
                }
            }
            if (i <= displayHearts) {
                Debug.Log("i " + i);
                heartsArray[i].enabled = true;
            }
            else {
                heartsArray[i].enabled = false;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        TakeAwayHeart();
        
    }
}
