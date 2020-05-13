using UnityEngine;
using UnityEngine.SceneManagement;

public class DesertDungeonEntranceController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Desert Dungeon");
        }
        
    }
}
