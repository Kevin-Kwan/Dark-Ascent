using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowCollider : MonoBehaviour
{
    public Material newMaterial;  // The material to change to.

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collission Enter");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Renderer renderer = collision.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = newMaterial;
            }
        }
    }
}
