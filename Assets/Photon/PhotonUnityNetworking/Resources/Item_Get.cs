using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Get : MonoBehaviour
{
    private bool ItemTrriger = false;    //隠れることが可能な場所かどうか

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

    }

    // Update is called once per frame
    void Update()
    {
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
}
