using UnityEngine.SceneManagement;
using UnityEngine;

public class ForestDungeonEntranceController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Forest Dungeon");
        }
        
    }
}
