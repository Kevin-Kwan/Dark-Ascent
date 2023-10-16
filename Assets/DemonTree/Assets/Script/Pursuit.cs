using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : MonoBehaviour
{
    public GameObject warden;
    public GameObject player;
    public bool evadeMode = false;

    private Vector3 velocity;
    public float Mass = 15;
    public float MaxVelocity = 3;
    public float MaxForce = 15;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero; 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredVelocity = player.transform.position - warden.transform.position;

        float pred = desiredVelocity.magnitude / MaxVelocity;
        desiredVelocity = desiredVelocity.normalized * MaxVelocity * pred;

        Vector3 steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, MaxForce);
        steering /= Mass;

        velocity = Vector3.ClampMagnitude(velocity + steering, MaxVelocity);
        warden.transform.position += velocity * Time.deltaTime;

        //warden.transform.position = new Vector3(warden.transform.position.x, 1, warden.transform.position.z);
        warden.transform.forward = velocity.normalized;

    }
}
