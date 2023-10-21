/*
 * File: ActivationCollector.cs
 * Author: Kevin Kwan
 * Created: 10/20/2023
 * Modified: 10/20/2023
 * Description: This script adds functionality to a trigger area that activates an object when all specified collectables in the area are collected.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationCollector : MonoBehaviour
{
    private int amountToCollect;
    public List<GameObject> collectables;
    public GameObject objectToActivate;

    private int collectedCount = 0;

    void Start()
    {
        amountToCollect = collectables.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        if (collectables.Contains(other.gameObject))
        {
            collectedCount++;
            collectables.Remove(other.gameObject);
            Destroy(other.gameObject);
        }

        if (collectedCount >= amountToCollect)
        {
            objectToActivate.SetActive(true);
        }
    }
}
