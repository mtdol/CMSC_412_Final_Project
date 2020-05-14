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
    public  string[] task;
    public string[] reward;
    private int n = 0;
    private bool CanvasOn = false;
    private bool nearbyhuh,finishhuh ,continuehuh, enableR ;
    
    void Start(){
        // start with canvas closed 
        canvas.SetActive(CanvasOn);
        finishhuh = false;
        continuehuh = false; 
        nearbyhuh = false; 
        PressR.SetActive(nearbyhuh);
        enableR = true;
        teleportPanel.SetActive(false);
        
        //Debug.Log(player.transform.position);
       
        
    }

    // slowly type out the text 
    IEnumerator Typetask(){
        foreach(char letter in task[n].ToCharArray()){
            textDisplay.text =  textDisplay.text + letter;
            yield return new WaitForSeconds(0.03f);
            
        }
    }

     IEnumerator Typereward(){
        foreach(char letter in reward[n].ToCharArray()){
            textDisplay.text =  textDisplay.text + letter;
            yield return new WaitForSeconds(0.03f);
            
        }
    }

    void Update(){ 

        if (player.transform.position.x < 74 && player.transform.position.x > 66
        && player.transform.position.z < 84 && player.transform.position.z > 76){
            nearbyhuh = true;
        }else{
            nearbyhuh =false;
        }
        PressR.SetActive(nearbyhuh && enableR);
        // 74 66 
        // 76 84 
        
        if ( CanvasOn == true && continuehuh == true){
            if (finishhuh == false ){
                StartCoroutine(Typetask());
                continuehuh = false;
            }else{
                StartCoroutine(Typereward());
                continuehuh = false;
            }
        }
        
        // press R to speak to npc 
        if (Input.GetKeyDown(KeyCode.R) && nearbyhuh == true) {
            enableR = false;
            CanvasOn = true;
            canvas.SetActive(CanvasOn);
            textDisplay.text =  "..!!??";
        }

        
        
        //press space to continue the conversation 
        if (Input.GetKeyDown(KeyCode.Y)) {
            //Debug.Log(n);
            continuehuh = true;
            //Debug.Log("space");
            if ( reward[n] != null && task[n] != null){
                textDisplay.text = "Villager: ";
                if (finishhuh == false ){
                    StartCoroutine(Typetask());
                    n = n + 1;
                    if(n == task.Length){
                        teleportPanel.SetActive(true);
                        n = 0;
                        enableR = true;
                        continuehuh = false;
                        finishhuh = true;
                        CanvasOn = false;
                        canvas.SetActive(CanvasOn);
                    }else{
                        continuehuh = false;}
                }else{
                    StartCoroutine(Typereward());
                    n = n + 1;
                    continuehuh = false;
                    if (n == reward.Length){
                        n = 0;
                        enableR = true;
                        continuehuh = false;
                        finishhuh = true;
                        CanvasOn = false;
                        canvas.SetActive(CanvasOn);
                    }else{
                        continuehuh = false;}
                }
            }
        }
       
        
    }


   
}
