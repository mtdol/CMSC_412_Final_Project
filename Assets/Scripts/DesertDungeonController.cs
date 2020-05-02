using UnityEngine;
using UnityEngine.SceneManagement;

public class DesertDungeonController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("collided");
            SceneManager.LoadScene("Desert Dungeon");
        }
        
    }
}
