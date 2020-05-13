using UnityEngine;

public class OverworldController : MonoBehaviour
{

    private GameObject player;
    private PlayerController playerController;

    // these are the places in the world the player can spawn at
    private GameObject defaultPlayerSpawn;
    private GameObject forestPlayerSpawn;
    private GameObject desertPlayerSpawn;

    // Start is called before the first frame update
    void Start()
    {
        defaultPlayerSpawn = GameObject.Find("Default Player Spawn");
        forestPlayerSpawn = GameObject.Find("Forest Player Spawn");
        desertPlayerSpawn = GameObject.Find("Desert Player Spawn");

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        // place the player at the static spawn point defined on the player
        if (playerController.GetPlayerSpawn() == PlayerController.DEFAULT_OVERWORLD_SPAWN)
        {
            player.transform.position = defaultPlayerSpawn.transform.position;
            player.transform.rotation = defaultPlayerSpawn.transform.rotation;
        } else if (playerController.GetPlayerSpawn() == PlayerController.DESERT_DUNGEON_SPAWN)
        {
            player.transform.position = desertPlayerSpawn.transform.position;
            player.transform.rotation = desertPlayerSpawn.transform.rotation;
        } else if (playerController.GetPlayerSpawn() == PlayerController.FOREST_DUNGEON_SPAWN)
        {
            player.transform.position = forestPlayerSpawn.transform.position;
            player.transform.rotation = forestPlayerSpawn.transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
