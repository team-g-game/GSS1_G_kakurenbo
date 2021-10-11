using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllPlayerCatch : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GameManager;  //Game_masterを入れる
    private Game_cont ScriptGameCont;   //Game_contの関数使えるようにする
    private Text AllCatch;

    void Start()
    {

        GameManager = GameObject.Find("Game_master");
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
        AllCatch = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ScriptGameCont.DemonFlag == true){
            if (Game_cont.Game_Status == Game_cont.Status.after){
                AllCatch.enabled = true;
            }
        }
    }
}
