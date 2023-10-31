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
        heartsArray = new Image[maxHearts];

        Transform heartsContainer = transform.Find("HeartContainer");
        for (int i = 0; i < maxHearts; i++) {
            heartsArray[i] = heartsContainer.Find("Heart" + (i + 1)).GetComponent<Image>();
        }
        TakeAwayHeart();
    }

    private void TakeAwayHeart() {
        float displayHearts = playerController.health / damagePerHeart;

        for (int i = 0; i < maxHearts; i++) {
            if (i <= displayHearts) {
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
