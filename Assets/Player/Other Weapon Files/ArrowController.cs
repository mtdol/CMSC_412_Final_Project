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
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

                print("made rigidbody");
            }

            // Move forward
            transform.Translate(new Vector3(0, 30, 0) * Time.deltaTime);
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.name.Equals("Player"))
        {
            print("Arrow has hit " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }
}
