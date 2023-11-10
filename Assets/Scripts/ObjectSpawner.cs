using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5.0f; // Set the time between spawns in seconds
    public float minRandomForce = 5f; // Minimum random force magnitude
    public float maxRandomForce = 15f; // Maximum random force magnitude
    public float inactivityThreshold = 1.0f; // Time in seconds to consider an object as inactive
    private float lowVelocityThreshold = 0.1f;
    private List<SpawnedObject> spawnedObjects = new List<SpawnedObject>();
    public Material objectMaterial; // Reference to the material to assign

    private class SpawnedObject
    {
        public GameObject gameObject;
        public float inactivityThreshold;

        public SpawnedObject(GameObject go, float threshold)
        {
            gameObject = go;
            inactivityThreshold = threshold;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnObjectsPeriodically());
    }

    private IEnumerator SpawnObjectsPeriodically()
    {
        while (true) // Spawn objects continuously
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnObject()
    {
        GameObject newObject = Instantiate(objectPrefab, spawnPoint.position, Quaternion.identity);

        Renderer renderer = newObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = newObject.AddComponent<MeshRenderer>();
        }

        // Assign the material to the renderer component
        renderer.material = objectMaterial;

        // Add a MeshCollider and set it to convex
        MeshCollider meshCollider = newObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;

        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = newObject.AddComponent<Rigidbody>(); // Add Rigidbody component if not already present
        }

        // Apply a force to make the object flow down the ramp
        Vector3 randomForce = new Vector3(0, 0, Random.Range(minRandomForce, maxRandomForce));
        rb.AddForce(randomForce, ForceMode.Impulse);

        // Set the "Hazard" tag
        newObject.tag = "Hazard";

        spawnedObjects.Add(new SpawnedObject(newObject, inactivityThreshold));

        // Start monitoring the object's velocity
        StartCoroutine(MonitorVelocityAndDestroy(newObject, inactivityThreshold));
    }

    
    private IEnumerator MonitorVelocityAndDestroy(GameObject obj, float inactivityThreshold)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        while (true)
        {
            yield return new WaitForSeconds(1.0f); // Check velocity every second

            if (rb.velocity.magnitude < lowVelocityThreshold)
            {
                if (inactivityThreshold <= 0f)
                {
                    spawnedObjects.RemoveAll(spawnedObj => spawnedObj.gameObject == obj);
                    Destroy(obj);
                    break;
                }
                else
                {
                    inactivityThreshold -= 1.0f;
                }
            }
        }
    }
}
