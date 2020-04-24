using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float forwardSpeed = 1f;
    public float backwardSpeed = 1f;
    public float strafeSpeed = 1f;

    // the main camera
    private GameObject cam;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("MainCamera");
        rb = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    
    private void HandleMovement()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        // adjust motion depending on forwards or backwards
        float forwardMotion;
        if (vAxis > 0)
        {
            forwardMotion = vAxis * forwardSpeed;
        }
        else
        {
            forwardMotion = vAxis * backwardSpeed;
        }

        // how fast we move to the side
        float strafeMotion = hAxis * strafeSpeed;

        // apply the movement
        Vector3 movement = new Vector3(strafeMotion, 0f, forwardMotion);
        rb.AddForce(movement);
    }
}
