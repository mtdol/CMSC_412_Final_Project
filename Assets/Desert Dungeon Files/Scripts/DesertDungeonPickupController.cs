using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertDungeonPickupController : MonoBehaviour
{
    private DesertDungeonController desertDungeonController;

    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        desertDungeonController = GameObject.Find("Desert Dungeon Controller").GetComponent<DesertDungeonController>();
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
            // let the dungeon controller know that we have been collected
            desertDungeonController.IncrementPickups();

            // deactivate ourself
            this.gameObject.SetActive(false);
        }
    }
}
