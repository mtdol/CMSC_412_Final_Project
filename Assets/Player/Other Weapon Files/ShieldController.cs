using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public bool guarding;

    // Start is called before the first frame update
    void Start()
    {
        // Don't want to detect collisions with the ground or player
        Collider playerCollider = GameObject.Find("Player").GetComponent<Collider>();
        Collider terrainCollider = GameObject.Find("Terrain").GetComponent<Collider>();
        Collider thisCollider = GetComponent<Collider>();
        Physics.IgnoreCollision(playerCollider, thisCollider);
        Physics.IgnoreCollision(terrainCollider, thisCollider);

        guarding = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (guarding)
        {
            // Do collision things
            print("Shield blocked " + collision.gameObject.name);
        }
    }
}
