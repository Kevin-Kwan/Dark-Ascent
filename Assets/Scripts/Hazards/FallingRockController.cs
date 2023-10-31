/*
 * File: FallingRock.cs
 * Author: Amal Chaudry
 * Created: 10/22/2023
 * Modified: 10/29/2023
 * Description: This script is the controller for the shaking animation state machine.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockController : MonoBehaviour
{
    public bool triggeredFall = false;
    public void TriggerFall() {
        triggeredFall = true;
    }
}
