using UnityEngine;
using UnityEngine.UI;

public class DesertDungeonController : MonoBehaviour
{

    private int pickupsCollected;
    public int pickupsRequired;

    private GameObject player;
    private PlayerController playerController;

    // the text display for the dungeon
    public Text dungeonStatusText;
    private StatusTextController dungeonStatusTextController;

    // Start is called before the first frame update
    void Start()
    {
        pickupsCollected = 0;

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();

        dungeonStatusTextController = dungeonStatusText.GetComponent<StatusTextController>();

        // set the opening message, telling the player to collect coins
        StartCoroutine(dungeonStatusTextController.SetTextTemporary("Collect All Coins in the Dungeon", "", 5));

        //DEBUG
        Debug.Log("entering desert dungeon");
        Debug.Log("Dungeon Status: " + playerController.GetDungeonCompletion(PlayerController.DESERT_DUNGEON));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // increments the number of pickups collected by the player
    public void IncrementPickups()
    {
        pickupsCollected++;
        if (pickupsCollected >= pickupsRequired)
        {
            WinDungeon();
        }
        else if (pickupsRequired - pickupsCollected == 3)
        {
            // notify the player they are almost done
            StartCoroutine(dungeonStatusTextController.SetTextTemporary("Only 3 Amulets Remain", "", 5));
        }
    }

    private void WinDungeon()
    {
        // tell the player controller that the player has beaten the dungeon
        playerController.SetDungeonCompletion(PlayerController.DESERT_DUNGEON, true);

        // set the screen, only lasts for a few seconds so use coroutine
        StartCoroutine(dungeonStatusTextController.SetTextTemporary("Dungeon Complete, Return to the Village", "", 5));
        
        //DEBUG
        Debug.Log("Dungeon Status: " + playerController.GetDungeonCompletion(PlayerController.DESERT_DUNGEON));
    }
}
