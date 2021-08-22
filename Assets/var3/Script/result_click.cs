using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class result_click : MonoBehaviour
{
    public Text decisiontext; 
    // Start is called before the first frame update
    void Start()
    {
        if(Game_cont.decision == 0) decisiontext.text = "母親の勝ち";
        else if (Game_cont.decision == 1) decisiontext.text = "ガキの勝ち"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void re_click(){
        GameObject.FindWithTag("scene_mane").GetComponent<Scene_mane_Script>().scene_num = 1;
        GameObject.FindWithTag("scene_mane").GetComponent<Scene_mane_Script>().scene_chanz = true;
    }
    public void exit(){
        GameObject.FindWithTag("scene_mane").GetComponent<Scene_mane_Script>().scene_num = 0;
        GameObject.FindWithTag("scene_mane").GetComponent<Scene_mane_Script>().scene_chanz = true;
    }
}
