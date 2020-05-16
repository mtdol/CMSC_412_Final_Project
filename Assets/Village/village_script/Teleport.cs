using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour{

    public GameObject teleportPanel;

    public void GoToDungon()
    {
        SceneManager.LoadScene(4);
    }

    public void CloseTeleportPanel(){
        teleportPanel.SetActive(false);
    }


}
