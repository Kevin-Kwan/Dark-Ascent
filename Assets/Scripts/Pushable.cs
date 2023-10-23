/*
 * File: Pushable.cs
 * Author: Kevin Kwan
 * Created: 10/20/2023
 * Modified: 10/20/2023
 * Description: This script adds functionality to pushable objects. They can respawn if they fall into the void or are destroyed in some way.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    Vector3 initialPosition;
    Vector3 initialRotation;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        transform.position = initialPosition;
        transform.eulerAngles = initialRotation;
    }
}
