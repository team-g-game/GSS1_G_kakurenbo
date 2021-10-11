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
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
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
                break;
            }
            case Game_cont.Status.after:{
                break;
            }
        }
        Pick_Up();
    }
    void Pick_Up()     //プレイヤーがアイテムを拾う時
    {
        if (ItemTrriger == true)
        {
            if (Input.GetKeyDown(KeyCode.F))//Fキーを押したときの判定
            {
                Debug.Log("ok");//アイテムの挙動
            }
        }
    }

    void FirstChangeTreasureChestList(){
        if (ScriptGameCont.CreateTreasureChestListflag == true && FirstChangeTreasureChestListFlag == false){
            ScriptGameCont.ChangeTreasureChestList(TreasureChestNumber, ItemsInfo);
            FirstChangeTreasureChestListFlag = true;
        }
    }
}
