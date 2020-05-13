using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldReturnController : MonoBehaviour
{
    private const string DESERT_DUNGEON_NAME = "Desert Dungeon";
    private const string FOREST_DUNGEON_NAME = "Forest Dungeon";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = GameObject.Find("Player");
            // set the spawn point according to the dungeon
            if (SceneManager.GetActiveScene().name == DESERT_DUNGEON_NAME)
            {
                player.GetComponent<PlayerController>().SetPlayerSpawn(PlayerController.DESERT_DUNGEON_SPAWN);
            }
            else if (SceneManager.GetActiveScene().name == FOREST_DUNGEON_NAME)
            {
                player.GetComponent<PlayerController>().SetPlayerSpawn(PlayerController.FOREST_DUNGEON_SPAWN);
            }

            SceneManager.LoadScene("OverWorld");
        }
        
    }
}
