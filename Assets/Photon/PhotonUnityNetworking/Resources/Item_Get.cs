using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Get : MonoBehaviour
{
    private bool ItemTrriger = false;    //隠れることが可能な場所かどうか
    public GameObject GameManager;  //Game_masterを入れる
    private Game_cont ScriptGameCont;   //Game_contの関数使えるようにする
    [SerializeField] private int TreasureChestNumber;
    [SerializeField] private string ItemsInfo = "000";
    private bool FirstChangeTreasureChestListFlag = false;
    GameObject TreasureChestObject;
    private bool OpenFlag = false;
    private float TimeCnt = 0.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Play_1")
        {
            ItemTrriger = true;
            this.GetComponent<MeshRenderer>().enabled=true;//メッシュレンダーの表示
        }
    }

    void OnTriggerExit(Collider pay)
    {
        if (pay.gameObject.name == "Play_1")
        {
            ItemTrriger = false;
            this.GetComponent<MeshRenderer>().enabled=false;//メッシュレンダーの非表示
        }
    }
    void Start()
    {
        GameManager = GameObject.Find("Game_master");
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
        TreasureChestObject = this.gameObject.transform.GetChild(0).gameObject;
                        Color color = TreasureChestObject.GetComponent<Renderer>().materials[0].color;
                Color color2 = TreasureChestObject.GetComponent<Renderer>().materials[1].color;
                color.a = 1;
                color2.a = 1;
                TreasureChestObject.GetComponent<Renderer>().materials[0].color = new Color32(0,0,0,0);
                TreasureChestObject.GetComponent<Renderer>().materials[1].color = new Color32(0,0,0,0);
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
                //TreasureChestOpened();
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
                UpdateTresureChestList();
                ScriptGameCont.ChangePlayerList(0, "ItemInfo", ItemsInfo);

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
        ScriptGameCont.ChangeTreasureChestList(TreasureChestNumber, "000");
        ScriptGameCont.SendTreasureChestList(TreasureChestNumber, "000");
    }

    void TreasureChestOpened(){
/*         string Infomation = ScriptGameCont.GetTreasureChestInfoFromIndex(TreasureChestNumber);
        if (Infomation[2] == '1'){
        } */
        if (OpenFlag == true){
            Color color = TreasureChestObject.GetComponent<Renderer>().materials[0].color;
            TimeCnt += Time.deltaTime;
            if (TimeCnt > 1.0f){
                TimeCnt = 1.0f;
            }
            color.a = TimeCnt;
            TreasureChestObject.GetComponent<Renderer>().materials[0].color = color;
        }
    }
}
