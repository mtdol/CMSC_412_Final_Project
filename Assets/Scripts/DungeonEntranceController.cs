using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

public class DungeonEntranceController : MonoBehaviour
{

    // if set to true, then a key will be required to load the dungeon scene
    public bool requireKey;

    // maps the current entrance with its associated scene
    // whenever a new entrance is added, a new pair must be added to this structure
    Dictionary<string, string> locations = new Dictionary<string, string> 
    {
        { "Forest Dungeon Entrance", "Forest Dungeon" },
        { "Desert Dungeon Entrance", "Desert Dungeon" },
        { "Village Entrance", "Village_main" },
    };

    // converts the dungeon entrance names to the appropriate player controller codes
    // when a new dungeon access code is added, this list must be updated
    Dictionary<string, int> dungeonCodes = new Dictionary<string, int>
    {
        {"Forest Dungeon Entrance", PlayerController.FOREST_DUNGEON},
        {"Desert Dungeon Entrance", PlayerController.DESERT_DUNGEON}, 
        {"Village Entrance", PlayerController.VILLAGE}, 

    };

    private OverworldController overworldController;

    void Start()
    {
        overworldController = GameObject.Find("Overworld Manager").GetComponent<OverworldController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerHasDungeonKey(other.gameObject))
            {
                // loads the appropriate scene using the dictionary above
                SceneManager.LoadScene(locations[this.gameObject.name]);
            }
            else
            {
                // the player needs the key
                StartCoroutine(
                    overworldController.GetStatusText().GetComponent<StatusTextController>().
                        SetTextTemporary("This Dungeon is locked, come back with a key from the village","",5)
                    );
            }
        }
        
    }

    private bool PlayerHasDungeonKey(GameObject player)
    {
        // checks if the current dungeon has dungeon access using records on the player controller
        // or returns true if player never requires a key to access the dungeon
        return player.GetComponent<PlayerController>().GetDungeonAccess(dungeonCodes[this.gameObject.name]) || !requireKey;
    }

}
