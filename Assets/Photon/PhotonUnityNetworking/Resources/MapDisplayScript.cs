using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 　


public class MapDisplayScript : MonoBehaviour
{
    bool MapDisplayed = false;
    bool SousaDisplayed = true;
    public GameObject GameManager;  //Game_masterを入れる
    private Game_cont ScriptGameCont;   //Game_contの関数使えるようにする

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("Game_master");
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
    }
    void Update()
    {
        switch (Game_cont.Game_Status){
            case Game_cont.Status.before:{
                MapDisplay(); 
                break;
            }
            case Game_cont.Status.play:{
                SousaGamen();
                MapDisplay();
                ScapeGoatDisplay();
                break;
            }
            case Game_cont.Status.after:{
                break;
            }
        }
    }


    void MapDisplay(){
        if (Input.GetKeyDown(KeyCode.M)){
            if (MapDisplayed == true){
                MapDisplayed = false;
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            else{
                MapDisplayed = true;
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    void SousaGamen(){
        if (Input.GetKeyDown(KeyCode.H)){
            if (SousaDisplayed == true){
                SousaDisplayed = false;
                this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            else{
                SousaDisplayed = true;
                this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    void ScapeGoatDisplay(){
        string scape = ScriptGameCont.GetPlayerInfoFromIndex(0, "ItemInfo");
        if (scape[0] == '1'){
        }
    }
}



