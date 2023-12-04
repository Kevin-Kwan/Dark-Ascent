/*
 * File: ToggleScript.cs
 * Author: Amal Chaudry
 * Created: 11/05/2023
 * Modified: 11/30/2023
 * Description: This script handles the toggling of camera controls.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    public Toggle cameraX;
    public Toggle cameraY;

    public GameObject player;
    public ThirdPController playerController;

    private bool isXInverted = false;
    private bool isYInverted = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("3rdPPlayer");
        playerController = player.GetComponent<ThirdPController>();
        playerController.invertMouseX = cameraX.isOn;
        playerController.invertMouseY = cameraY.isOn;
    }

    // Update is called once per frame
    void Update()
    {
        playerController.invertMouseX = cameraX.isOn;
        playerController.invertMouseY = cameraY.isOn;
        
    }
}
