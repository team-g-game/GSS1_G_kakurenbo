using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchText : MonoBehaviour
{    
    private float totalTime;    //経過全体時間
    [SerializeField] private int DisplayTimeMinute;     //表示したい時間分
	[SerializeField] private float DisplayTimeSeconds;  //表示したい時間秒
    private int Minute;                     //経過全体時間分
    private float Seconds;                  //経過全体時間秒
	//　前回Update時の秒数
    private Text Catch;
    public GameObject GameManager;  //Game_masterを入れる
    private Game_cont ScriptGameCont;   //Game_contの関数使えるようにする


    // Start is called before the first frame update
    void Start()
    {
        totalTime = DisplayTimeMinute * 60 + DisplayTimeSeconds;
        Minute = DisplayTimeMinute;
        Seconds = DisplayTimeSeconds;
        Catch = GetComponentInChildren<Text>();
        GameManager = GameObject.Find("Game_master");
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ScriptGameCont.DemonFlag == true){
            if (ScriptGameCont.CatchPlayerFlag == true){
                totalTime = Minute * 60 + Seconds;
                totalTime -= Time.deltaTime;

                Minute = (int)totalTime / 60;
                Seconds = totalTime - Minute * 60;
                if (totalTime <= 0f){
                    Catch.enabled = false;
                    ScriptGameCont.CatchPlayerFlag = false;
                    Minute = DisplayTimeMinute;
                    Seconds = DisplayTimeSeconds;
                }
                else{
                    Catch.enabled = true;
                }
            }
        }
    }
}
