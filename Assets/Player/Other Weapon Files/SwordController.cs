using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        // Don't want to detect collisions with the ground or player
        Collider playerCollider = GameObject.Find("Player").GetComponent<Collider>();
        Collider terrainCollider = GameObject.Find("Terrain").GetComponent<Collider>();
        Collider thisCollider = GetComponent<Collider>();
        Physics.IgnoreCollision(playerCollider, thisCollider);
        Physics.IgnoreCollision(terrainCollider, thisCollider);

        attacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (attacking)
        {
            // Do collider things
            print("Sword sliced " + collision.gameObject.name);
        }
    }
}
