using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class result_click : MonoBehaviour
{
    public Text decisiontext; 
    // Start is called before the first frame update
    void Start()
    {
        if(Game_cont.decision == 0) decisiontext.text = "母親の勝ち!";
        else if (Game_cont.decision == 1) decisiontext.text = "ガキの勝ち!"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void exit(){
      SceneManager.LoadScene ("title");
    }
}
