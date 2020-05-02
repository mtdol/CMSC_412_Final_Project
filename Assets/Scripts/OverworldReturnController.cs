using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldReturnController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("collided");
            SceneManager.LoadScene("OverWorld");
        }
        
    }
}
