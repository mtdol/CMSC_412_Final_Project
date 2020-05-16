using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeakToNPC : MonoBehaviour {

    public GameObject canvas;
    public GameObject PressR;
    public GameObject player;
    public GameObject teleportPanel; 
    public TextMeshProUGUI textDisplay;
    public  string[] AskMaze, FinishMaze, FinishForest, FinishDessert ;

    private int n = 0;
    private bool CanvasOn = false;
    private bool nearbyhuh , enableR ;
    
    void Start(){
        // start with canvas closed 
        canvas.SetActive(CanvasOn);

        nearbyhuh = false; 
        PressR.SetActive(nearbyhuh);
        enableR = true;
        teleportPanel.SetActive(false);
        
        //Debug.Log(player.transform.position);

        //controllerScript cs = player.GetComponent<PlayerController>();
       
        
    }

    // slowly type out the text 
    IEnumerator Typetask(){
        foreach(char letter in AskMaze[n].ToCharArray()){
            textDisplay.text =  textDisplay.text + letter;
            yield return new WaitForSeconds(0.03f);
            
        }
    }

     IEnumerator Typereward(){
        foreach(char letter in FinishMaze[n].ToCharArray()){
            textDisplay.text =  textDisplay.text + letter;
            yield return new WaitForSeconds(0.03f);
            
        }
    }

     IEnumerator TypeForest(){
        foreach(char letter in FinishForest[n].ToCharArray()){
            textDisplay.text =  textDisplay.text + letter;
            yield return new WaitForSeconds(0.03f);
            
        }
    }
         IEnumerator TypeDessert(){
        foreach(char letter in FinishDessert[n].ToCharArray()){
            textDisplay.text =  textDisplay.text + letter;
            yield return new WaitForSeconds(0.03f);
            
        }
    }

    void Update(){ 

        bool HaveKey = PlayerController.haveMazeKey;
        // bool CompleteForest = PlayerController.dungeonCompletion[0];
        // Debug.Log("forest complete" + CompleteForest);
        // bool CompleteDessert = PlayerController.dungeonCompletion[1];
        // Debug.Log("dessert complete" + CompleteDessert);



        //Debug.Log("do i have key " + HaveKey);

        if (player.transform.position.x < 74 && player.transform.position.x > 66
        && player.transform.position.z < 84 && player.transform.position.z > 76){
            nearbyhuh = true;
        }else{
            nearbyhuh =false;
        }
        PressR.SetActive(nearbyhuh && enableR);

        

        
        // press R to speak to npc 
        if (Input.GetKeyDown(KeyCode.R) && nearbyhuh == true) {
            enableR = false;
            CanvasOn = true;
            canvas.SetActive(CanvasOn);
            textDisplay.text =  "..!!??";
        }


        // if (Input.GetKeyDown(KeyCode.U) ) {
        //     PlayerController.dungeonCompletion[0] = true;
        //     Debug.Log("access to forest are now set to:" +  PlayerController.dungeonCompletion[0]);
        // }

        // if (Input.GetKeyDown(KeyCode.I) ) {
        //     PlayerController.dungeonCompletion[1] = true;
        //     Debug.Log("access to dessert are now set to:" +  PlayerController.dungeonCompletion[1]);
        // }



        
        
        //press space to continue the conversation 
        if (Input.GetKeyDown(KeyCode.Y)) {
            //Debug.Log(n);

            //Debug.Log("space");
            if ( FinishMaze[n] != null && AskMaze[n] != null){
                textDisplay.text = "Villager: ";
                //maze 
                if (HaveKey == false && PlayerController.dungeonCompletion[0]== false && PlayerController.dungeonCompletion[1]== false){
                    StartCoroutine(Typetask());
                    n = n + 1;
                    if(n == AskMaze.Length){
                        teleportPanel.SetActive(true);
                        n = 0;
                        enableR = true;
                        CanvasOn = false;
                        canvas.SetActive(CanvasOn);
                    }
                //forest dungon 
                }else if(HaveKey == true && PlayerController.dungeonCompletion[0]== false && PlayerController.dungeonCompletion[1]== false ) {
                    StartCoroutine(Typereward());
                    n = n + 1;
                    if (n == FinishMaze.Length){
                        PlayerController.dungeonAccess[0] = true;
                        //Debug.Log("after maze " + PlayerController.dungeonAccess[0]);
                        n = 0;
                        enableR = true;

                        CanvasOn = false;
                        canvas.SetActive(CanvasOn);     
                    }
                //dessert dungon
                }else if(HaveKey == true && PlayerController.dungeonCompletion[0]== true && PlayerController.dungeonCompletion[1]== false){
                    StartCoroutine(TypeForest());
                    n = n + 1;
                    if (n == FinishForest.Length){
                        PlayerController.dungeonAccess[1] = true;
                        //Debug.Log("after maze " + PlayerController.dungeonAccess[1]);
                        n = 0;
                        enableR = true;
                        CanvasOn = false;
                        canvas.SetActive(CanvasOn);     
                    }
                //beat the game 
                }else if(HaveKey == true && PlayerController.dungeonCompletion[0]== true && PlayerController.dungeonCompletion[1]== true){
                    StartCoroutine(TypeDessert());
                    n = n + 1;
                    if (n == FinishDessert.Length){
                        n = 0;
                        enableR = true;
                        CanvasOn = false;
                        canvas.SetActive(CanvasOn);     
                    }

                }
            }
        }
       
        
    }


   
}
