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
    GameObject[] DisplayUi = new GameObject[4];

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("Game_master");
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
        MapDisplayed = false;
        SousaDisplayed = false;
        for (int i = 0; i < 4; i++){
            DisplayUi[i] = this.gameObject.transform.GetChild(i).gameObject;
            this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
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
                    CatchGaki();
                    ScapeGoatDisplay();
                }
                break;
            }
            case Game_cont.Status.after:{
                break;
            }
        }
    }


    /// <summary>
    /// マップ表示
    /// </summary>
    void MapDisplay(){
        if (Input.GetKeyDown(KeyCode.M)){
            MapDisplayed = this.gameObject.transform.GetChild(0).gameObject.activeSelf;
            if (MapDisplayed == true){
                MapDisplayed = false;
                DisplayUi[0].SetActive(false);
            }
            else{
                MapDisplayed = true;
                DisplayUi[0].SetActive(true);
            }
        }
    }

    /// <summary>
    /// 操作説明の画像を表示
    /// </summary>
    void SousaGamen(){
        if (Input.GetKeyDown(KeyCode.H)){
            SousaDisplayed = this.gameObject.transform.GetChild(1).gameObject.activeSelf;
            if (SousaDisplayed == true){
                SousaDisplayed = false;
                DisplayUi[1].SetActive(false);
            }
            else{
                SousaDisplayed = true;
                DisplayUi[1].SetActive(true);
            }
        }
    }


    /// <summary>
    /// スケープゴートで見つからなかったことを表示する
    /// </summary>
    void ScapeGoatDisplay(){
        string Scape = ScriptGameCont.GetPlayerInfoFromIndex(0, "ItemInfo");
        if (Scape[0] == '1'){
            DisplayUi[2].SetActive(true);
        }
        else{
            DisplayUi[2].SetActive(false);
        }
    }
    
    /// <summary>
    /// 見つかったということの表示
    /// </summary>
    void CatchGaki(){
        string Catch = ScriptGameCont.GetPlayerInfoFromIndex(0, "CatchFlag");
        if (Catch == "True"){
            DisplayUi[3].SetActive(true);
        }
        else if (Catch == "False"){
            DisplayUi[3].SetActive(false);
        }
    }
}



