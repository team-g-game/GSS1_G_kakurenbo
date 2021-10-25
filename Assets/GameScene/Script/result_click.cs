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
        if(Game_cont.decision) decisiontext.text = "ガキの勝ち!";
        else decisiontext.text = "母親の勝ち!"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void exit(){
      SceneManager.LoadScene ("title");
      Game_cont.Game_Status = Game_cont.Status.before;
      Destroy(GameObject.FindWithTag("game_manegyr").gameObject,0f);
    }
}
