using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public bool released = false;

    void FixedUpdate()
    {
        // If we've been fired, do things...
        if (released)
        {
            // If this is the first frame where we've been fired, detatch ourselves from the bow/player 
            // and create our own rigidbody (so we can collide with things)
            if (transform.parent != null)
            {
                transform.SetParent(null);
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.freezeRotation = true;
            }

            // Move forward
            transform.Translate(new Vector3(0, 15, 0) * Time.deltaTime);
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Do collision things
        print("Hello?");

        if (!collision.gameObject.name.Equals("Player"))
        {
            Destroy(gameObject);
        }
    }
}
