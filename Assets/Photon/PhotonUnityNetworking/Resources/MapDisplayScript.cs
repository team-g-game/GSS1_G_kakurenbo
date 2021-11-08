using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 　
using Photon.Pun;
using Photon;
using Photon.Realtime;


public class MapDisplayScript : MonoBehaviour
{
    bool MapDisplayed = true;
    bool SousaDisplayed = true;
    public GameObject GameManager;  //Game_masterを入れる
    private Game_cont ScriptGameCont;   //Game_contの関数使えるようにする

    GameObject[] DisplayUi = new GameObject[4];
    [SerializeField] private GameObject PhotonHontai;
    private PhotonView view = null;

    // Start is called before the first frame update
    void Start()
    {
        view = PhotonHontai.GetComponent<PhotonView>();
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
                if(view.IsMine){
                    SousaGamen();
                    MapDisplay();
                    if (ScriptGameCont.DemonFlag == false){
                        CatchGaki();
                        ScapeGoatDisplay();
                    }
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
            DisplayUi[0] = this.gameObject.transform.GetChild(0).gameObject;
            MapDisplayed = DisplayUi[0].activeSelf;
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
            DisplayUi[1] = this.gameObject.transform.GetChild(1).gameObject;
            SousaDisplayed = DisplayUi[1].activeSelf;
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
        DisplayUi[2] = this.gameObject.transform.GetChild(2).gameObject;
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
        DisplayUi[3] = this.gameObject.transform.GetChild(3).gameObject;
        if (Catch == "True"){
            DisplayUi[3].SetActive(true);
        }
        else if (Catch == "False"){
            DisplayUi[3].SetActive(false);
        }
    }
}



