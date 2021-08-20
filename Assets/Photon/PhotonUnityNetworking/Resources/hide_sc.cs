using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class hide_sc : MonoBehaviour
{
    // Start is called before the first frame update
    private bool HideTrriger = false;    //隠れることが可能な場所かどうか
    private bool WatchTrriger = false;   //見ることが可能な場所かどうか
    public bool Hidestate = false;      //隠れている状態かどうか  ここがネットワーク経由で変更されるとうれしい
    Vector3 Hide_before_pos;            //隠れる前のプレイヤーの位置
    GameObject Player_obj;              //プレイヤーの子オブジェクト
    GameObject Parent_Player_obj;       //プレイヤーの親オブジェクト
    private int Hide_Place = 0;         //アタッチしたものがどれか判別する
    public bool Scapegoat_bool = false; //スケープゴート持ってるかどうか
    [SerializeField] private int HidePlaceNum = 0;
    private PhotonView View = null;
    public GameObject GameManager;  //Game_masterを入れる
    private Game_cont ScriptGameCont;   //Game_contの関数使えるようにする
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Play_1")
        {
            HideTrriger = true;
            this.GetComponent<MeshRenderer>().enabled=true;//メッシュレンダーの表示
            Player_obj = other.gameObject;                                  //Play_1
            Parent_Player_obj = other.transform.parent.gameObject;          //Play_1の親


            //ここでそのプレイヤーがスケープゴート持ってるかどうか持ってきたい
        }
        if (other.gameObject.name == "Cylinder")
        {
            this.GetComponent<TextMesh> ().text = "Watch";  //text meshのテキストを変更
            WatchTrriger = true;
            this.GetComponent<MeshRenderer>().enabled=true;//メッシュレンダーの表示
        }
    }

    void OnTriggerExit(Collider pay)
    {
        if (pay.gameObject.name == "Play_1")
        {
            HideTrriger = false;
            this.GetComponent<MeshRenderer>().enabled=false;//メッシュレンダーの非表示
        }
        if (pay.gameObject.name == "Cylinder")
        {
            WatchTrriger = false;
            this.GetComponent<MeshRenderer>().enabled=false;//メッシュレンダーの非表示
        }
    }
    void Start()
    {
        if (this.tag == "treehouse"){Hide_Place = 1;}       //ツリーハウスだった時
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
    }

    // Update is called once per frame
    void Update()
    {
        Hide();
        Watch();
    }
    void Hide()     //プレイヤーが隠れる時
    {
        if (HideTrriger == true)
        {
            if (Input.GetKeyDown(KeyCode.E))//Eキーを押したときの判定
            {
                Debug.Log("ok");//隠れる場所にカメラ移動させる物を後で追加
                Debug.Log($"{Parent_Player_obj.name}");
                if (Hidestate == false)             //隠れていないとき
                {
                    UpdateHidePlaceNumber(HidePlaceNum);
                    Hidestate = true;
                    VisualTrriger(Player_obj, false);
                    Hide_before_pos = Parent_Player_obj.transform.position;         //プレイヤーの隠れる前の座標保持
                    if (Hide_Place == 1){                                           //ツリーハウスのときの座標変更
                        var pos = this.transform.position;
                        pos.x += 20;
                        pos.y += 35;
                        pos.z += 5;
                        Parent_Player_obj.transform.position = pos;
                    }
                    else if (Hide_Place == 0){                                      //そのほかの時の座標変更
                        Parent_Player_obj.transform.position = this.transform.position; //プレイヤーを隠れる場所の座標に変更
                    }
                }
                else if(Hidestate == true)          //隠れているとき
                {
                    UpdateHidePlaceNumber(0);
                    Hidestate = false;
                    VisualTrriger(Player_obj, true);
                    Parent_Player_obj.transform.position = Hide_before_pos;         //プレイヤーの位置を戻す
                }
            }
        }
    }

    void Watch()        //鬼が見つけるとき
    {
        if (WatchTrriger == true)       //隠れている場所を見ることができる
        {
            if (Input.GetKeyDown(KeyCode.E))    //Eボタン押したとき
            {
                if (Hidestate == true){          //隠れているなら
                    if (Scapegoat_bool == true){ //スケープゴート持ってるなら
                        Debug.Log("いないよ");
                    }
                    else {
                        for (int i = 0; i < ScriptGameCont.GetPlayerInfoListCount(); ++i){
                            if (HidePlaceNum.ToString() == ScriptGameCont.GetPlayerInfoFromIndex(i, "HidePlace")){
                                Debug.Log("見つけた");
                                string PlayerViewId = ScriptGameCont.GetPlayerViewIdFromListByIndex(i).ToString();
                                ScriptGameCont.UpdatePlayerInfoAndHash(PlayerViewId,"CatchFlag", "true");
                                ScriptGameCont.UpdatePlayerInfoAndHash(PlayerViewId,"HidePlace", "100");
                                Debug.Log(ScriptGameCont.GetPlayerInfo(PlayerViewId,"CatchFlag"));
                                Debug.Log(ScriptGameCont.GetPlayerInfo(PlayerViewId,"HidePlace"));
                            }
                        }
                    }

                }
                else                            //いないなら
                {
                    Debug.Log("いないよ");
                }
            }
        }
    }

    /// <summary>
    /// 隠れる場所の番号を指定してプレイヤー情報の更新とオンラインにハッシュを送信。
    /// </summary>
    /// <param name="HidePlaceNumber">隠れる場所の識別番号</param>
    void UpdateHidePlaceNumber(int HidePlaceNumber){
        View = Parent_Player_obj.GetComponent<PhotonView>();
        int Index = ScriptGameCont.GetPlayerInfoIndexFromViewId(View.ViewID.ToString());
        ScriptGameCont.ChangePlayerList(Index, "HidePlace", HidePlaceNumber.ToString());
        ScriptGameCont.SendPlayerInfo(Index);
        string place = ScriptGameCont.GetPlayerInfo(View.ViewID.ToString(), "HidePlace");
        Debug.Log(place);
    }

    /// <summary>
    /// プレイヤーのキャラクターを表示するか、非表示にするか
    /// </summary>
    /// <param name="HidePlayerCharacter">trueが表示、falseが非表示</param>
    public static void VisualTrriger(GameObject PlayerObj, bool HidePlayerCharacter){
        if(HidePlayerCharacter == true){
            PlayerObj.GetComponent<Rigidbody>().isKinematic=false;         //重力オン
            PlayerObj.GetComponent<BoxCollider>().enabled=true;            //コライダーつける
            PlayerObj.GetComponent<MeshRenderer>().enabled=true;           //プレイヤー見える
        }
        else{
            PlayerObj.GetComponent<Rigidbody>().isKinematic=true;          //重力無視
            PlayerObj.GetComponent<BoxCollider>().enabled=false;           //コライダー消す
            PlayerObj.GetComponent<MeshRenderer>().enabled=false;          //プレイヤー見えなくする
        }
    }
}
