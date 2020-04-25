using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float forwardSpeed = 1f;
    public float backwardSpeed = 1f;
    public float strafeSpeed = 1f;
    public float rotationSpeed = 1f;
    public float sprintSpeed = 2f;

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
        float rawRotation = Input.GetAxis("Mouse X");

        // adjust motion depending on forwards or backwards
        float forwardMotion;
        if (vAxis > 0)
        {
            forwardMotion = vAxis * forwardSpeed * Time.fixedDeltaTime;
        }
        else
        {
            forwardMotion = vAxis * backwardSpeed * Time.fixedDeltaTime;
        }

        // how fast we move to the side
        float strafeMotion = -hAxis * strafeSpeed * Time.fixedDeltaTime;

        float rotation = rawRotation * rotationSpeed * Time.fixedDeltaTime;

        // check if we are running
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // only run if we are going forward
            if (vAxis > 0)
            {
                forwardMotion *= sprintSpeed;
            }
        }

        // apply rotation
        this.transform.Rotate(0, rotation, 0);

        // apply the movement
        Vector3 movement = new Vector3(forwardMotion, 0f, strafeMotion);
        // ensure that we rotate correctly
        movement = this.transform.TransformDirection(movement);
        rb.MovePosition(this.transform.position + movement);
    }
}
