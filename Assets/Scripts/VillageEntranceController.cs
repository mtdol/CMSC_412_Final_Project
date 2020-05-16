using UnityEngine.SceneManagement;
using UnityEngine;

public class VillageEntranceController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Village_main");
        }
        
    }
}
