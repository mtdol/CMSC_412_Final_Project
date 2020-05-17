using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestDungeonPickupController : MonoBehaviour
{
    private ForestDungeonController forestDungeonController;

    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        forestDungeonController = GameObject.Find("Forest Dungeon Controller").GetComponent<ForestDungeonController>();
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
            forestDungeonController.IncrementPickups();

            // deactivate ourself
            this.gameObject.SetActive(false);
        }
    }
}
