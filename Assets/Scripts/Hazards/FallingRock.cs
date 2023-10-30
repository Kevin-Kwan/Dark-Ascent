/*
 * File: FallingRock.cs
 * Author: Amal Chaudry
 * Created: 10/22/2023
 * Modified: 10/29/2023
 * Description: This script causes a rock platform to shake and fall.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    public GameObject player;
    public GameObject rock;

    public Animator anim;
    public Rigidbody rockRigidbody;
    public bool onRock;

    public float shakeDuration = 2.0f; //how long it shakes
    public float shakeTimer;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("3rdPPlayer");
        rock = GameObject.Find("FallingRock");
        anim = rock.GetComponent<Animator>();
        rockRigidbody = rock.GetComponent<Rigidbody>();
        rockRigidbody.isKinematic = true;
        onRock = false;
        initialPosition = transform.parent.position;
    }

    void Update() {
        if (onRock) {
            shakeTimer += Time.deltaTime;

            if (shakeTimer >= shakeDuration) {
                rockRigidbody.isKinematic = false;
                rockRigidbody.useGravity = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            Debug.Log("player on rock");
            onRock = true;
            FallingRockController frc = rock.gameObject.GetComponent<FallingRockController>();
            if (frc != null) {
                anim.SetBool("onRock", true);
            }

        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == player) {
            Debug.Log("player left rock");
            onRock = false;
        }
    }
}
