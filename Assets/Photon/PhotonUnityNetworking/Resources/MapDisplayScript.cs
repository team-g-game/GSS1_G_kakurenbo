using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 　


public class MapDisplayScript : MonoBehaviour
{
    bool MapDisplayed = true;
    bool SousaDisplayed = true;
    public GameObject GameManager;  //Game_masterを入れる
    private Game_cont ScriptGameCont;   //Game_contの関数使えるようにする

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("Game_master");
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
        MapDisplayed = false;
        SousaDisplayed = false;
    }
    void Update()
    {
        switch (Game_cont.Game_Status){
            case Game_cont.Status.before:{
                //MapDisplay(); 
                break;
            }
            case Game_cont.Status.play:{
                SousaGamen();
                MapDisplay();
                if (ScriptGameCont.DemonFlag == false){
                    ScapeGoatDisplay();
                }
                break;
            }
            case Game_cont.Status.after:{
                break;
            }
        }
    }


    void MapDisplay(){
        if (Input.GetKeyDown(KeyCode.M)){
            MapDisplayed = this.gameObject.transform.GetChild(0).gameObject.activeSelf;
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
            SousaDisplayed = this.gameObject.transform.GetChild(1).gameObject.activeSelf;
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
            this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        else{
            this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}



