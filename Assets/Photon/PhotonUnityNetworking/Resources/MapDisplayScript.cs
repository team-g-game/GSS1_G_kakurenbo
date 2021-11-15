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

    GameObject[] DisplayUi = new GameObject[5];
    [SerializeField] private GameObject PhotonHontai;
    private PhotonView view = null;
    private float totalTime;    //経過全体時間
    private int DisplayTimeMinute = 0;     //表示したい時間分
	private float DisplayTimeSeconds = 1;  //表示したい時間秒
    private int Minute;                     //経過全体時間分
    private float Seconds;                  //経過全体時間秒
    bool DisplayNoItemFlag = false;                     //アイテムがないという表示をしたかどうか

    // Start is called before the first frame update
    void Start()
    {
        view = PhotonHontai.GetComponent<PhotonView>();
        GameManager = GameObject.Find("Game_master");
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
        MapDisplayed = false;
        SousaDisplayed = false;
        totalTime = DisplayTimeMinute * 60 + DisplayTimeSeconds;
        Minute = DisplayTimeMinute;
        Seconds = DisplayTimeSeconds;
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
                        DisplayNoItem();
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

    /// <summary>
    /// アイテムがない宝箱を開けたときに表示する関数
    /// </summary>
    public void GetNoItem(){
        if (ScriptGameCont.DemonFlag == false){
            DisplayUi[4] = this.gameObject.transform.GetChild(4).gameObject;
            DisplayUi[4].SetActive(true);
            DisplayNoItemFlag = true;
        }
    }

    /// <summary>
    /// アイテムがないことを表示した後に秒数指定して消す
    /// </summary>
    void DisplayNoItem(){
        if (DisplayNoItemFlag == true){
            totalTime = Minute * 60 + Seconds;
            totalTime -= Time.deltaTime;

            Minute = (int)totalTime / 60;
            Seconds = totalTime - Minute * 60;
            if (totalTime <= 0f){       //設定した時間がたったら
                DisplayUi[4].SetActive(false);
                Debug.Log("kitenai");
                DisplayNoItemFlag = false;
                Minute = DisplayTimeMinute;
                Seconds = DisplayTimeSeconds;
            }
        }
    }
}



