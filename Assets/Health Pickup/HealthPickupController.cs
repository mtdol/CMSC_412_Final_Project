using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupController : MonoBehaviour
{
    public float rotationSpeed;
    public int healthIncrease;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // rotate
        transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // heal the player
            other.gameObject.GetComponent<PlayerController>().Heal(healthIncrease);

            // deactivate ourself
            this.gameObject.SetActive(false);
        }
    }
}
