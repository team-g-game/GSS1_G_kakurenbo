using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Item_Get : MonoBehaviour
{
    private bool ItemTrriger = false;    //隠れることが可能な場所かどうか
    public GameObject GameManager;  //Game_masterを入れる
    private Game_cont ScriptGameCont;   //Game_contの関数使えるようにする
    GameObject Player_obj;              //プレイヤーの子オブジェクト
    GameObject Parent_Player_obj;       //プレイヤーの親オブジェクト
    private PhotonView View = null;
    [SerializeField] private int TreasureChestNumber;       //宝箱の番号
    [SerializeField] private string ItemsInfo;              //入ってるアイテム情報
    private bool FirstChangeTreasureChestListFlag = false;  //
    GameObject TreasureChestObject;
    private bool OpenFlag = false;
    private GameObject MapDisplay;              //MapDisplayのGameobject
    private MapDisplayScript ScriptMapDiaplay;  //MapDisplayのスクリプト
    public AudioClip sound1;
    AudioSource audioSource;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Play_1")
        {
            Player_obj = other.gameObject;                                  //Play_1
            Parent_Player_obj = other.transform.parent.gameObject;          //Play_1の親
            View = Parent_Player_obj.GetComponent<PhotonView>();
            MapDisplay = Parent_Player_obj.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
            ScriptMapDiaplay = MapDisplay.GetComponent<MapDisplayScript>();
            if (View.IsMine){           //自分のときだけ実行するようにしないといけない
                ItemTrriger = true;
                this.GetComponent<MeshRenderer>().enabled=true;     //メッシュレンダーの表示
            }
        }
    }

    void OnTriggerExit(Collider pay)
    {
        if (pay.gameObject.name == "Play_1")
        {
            if(View.IsMine){
                ItemTrriger = false;
                this.GetComponent<MeshRenderer>().enabled=false;    //メッシュレンダーの非表示
            }
        }
    }
    void Start()
    {
        GameManager = GameObject.Find("Game_master");
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
        TreasureChestObject = this.gameObject.transform.GetChild(0).gameObject;
        audioSource = GetComponent<AudioSource>();      //オーディオソースをゲット
    }

    // Update is called once per frame
    void Update()
    {
        switch (Game_cont.Game_Status){
            case Game_cont.Status.before:{         
                FirstChangeTreasureChestList();
                break;
            }
            case Game_cont.Status.play:{
                Pick_Up();
                TreasureChestOpened();
                break;
            }
            case Game_cont.Status.after:{
                break;
            }
        }
    }
    void Pick_Up()     //プレイヤーがアイテムを拾う時
    {
        if (ItemTrriger == true)
        {
            if (Input.GetKeyDown(KeyCode.F))//Fキーを押したときの判定
            {
                Debug.Log("ItemGet");//アイテムの挙動
                if(ItemsInfo == "000"){
                    ScriptMapDiaplay.GetNoItem();
                }
                string Items = ScriptGameCont.GetPlayerInfoFromIndex(0, "ItemInfo");
                if (Items[0] == '0'){
                    ScriptGameCont.ChangePlayerList(0, "ItemInfo", ItemsInfo);
                    ScriptGameCont.SendPlayerInfo(0);
                }
                UpdateTresureChestList();
                TreasureChestObject.GetComponent<MeshRenderer>().enabled = false;
                TreasureChestObject.SetActive(false);
                this.GetComponent<BoxCollider>().enabled = false;
                this.GetComponent<MeshRenderer>().enabled = false;
                audioSource.PlayOneShot(sound1);
                OpenFlag = true;
            }
        }
    }

    /// <summary>
    /// 初期値をTreasureChestListに挿入。
    /// </summary>
    void FirstChangeTreasureChestList(){
        if (ScriptGameCont.CreateTreasureChestListflag == true && FirstChangeTreasureChestListFlag == false){
            ScriptGameCont.ChangeTreasureChestList(TreasureChestNumber, ItemsInfo);
            FirstChangeTreasureChestListFlag = true;
        }
    }

    /// <summary>
    /// TreasureChestListを更新して、ハッシュに送信。
    /// </summary>
    void UpdateTresureChestList(){
        ScriptGameCont.ChangeTreasureChestList(TreasureChestNumber, "001");
        ScriptGameCont.SendTreasureChestList(TreasureChestNumber, "001");
    }

    void TreasureChestOpened(){
        if (OpenFlag == false){ //開いてないとき
            string Infomation = ScriptGameCont.GetTreasureChestInfoFromIndex(TreasureChestNumber);
            if (Infomation[2] == '1'){
                TreasureChestObject.GetComponent<MeshRenderer>().enabled = false;
                TreasureChestObject.SetActive(false);
                this.GetComponent<BoxCollider>().enabled = false;
                this.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
