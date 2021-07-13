using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hide_sc : MonoBehaviour
{
    // Start is called before the first frame update
    public bool HideTrriger = false;    //隠れることが可能な場所かどうか
    public bool WatchTrriger = false;   //見ることが可能な場所かどうか
    public bool Hidestate = false;      //隠れている状態かどうか
    Vector3 Hide_before_pos;            //隠れる前のプレイヤーの位置
    GameObject Player_obj;              //プレイヤーの子オブジェクト
    GameObject Parent_Player_obj;       //プレイヤーの親オブジェクト
    private int Hide_Place = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Play_1")
        {
            HideTrriger = true;
            this.GetComponent<MeshRenderer>().enabled=true;//メッシュレンダーの表示
            Player_obj = other.gameObject;                            
            Parent_Player_obj = other.transform.parent.gameObject;
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
        if (this.tag == "treehouse"){
            Hide_Place = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        hide();
    }
    void hide()
    {
        if (HideTrriger == true)
        {
            if (Input.GetKeyDown(KeyCode.E))//Eキーを押したときの判定
            {
                Debug.Log("ok");//隠れる場所にカメラ移動させる物を後で追加
                Debug.Log($"{Parent_Player_obj.name}");
                if (Hidestate == false)             //隠れていないとき
                {
                    Hidestate = true;
                    Player_obj.GetComponent<Rigidbody>().isKinematic=true;          //重力無視
                    Player_obj.GetComponent<BoxCollider>().enabled=false;           //コライダー消す
                    Player_obj.GetComponent<MeshRenderer>().enabled=false;          //プレイヤー見えなくする
                    Hide_before_pos = Parent_Player_obj.transform.position;         //プレイヤーの隠れる前の座標保持
                    if (Hide_Place == 1){
                        var pos = this.transform.position;
                        pos.x += 20;
                        pos.y += 35;
                        pos.z += 5;
                        Parent_Player_obj.transform.position = pos;
                    }
                    else if (Hide_Place == 0){
                        Parent_Player_obj.transform.position = this.transform.position; //プレイヤーを隠れる場所の座標に変更
                    }
                }
                else if(Hidestate == true)          //隠れているとき
                {
                    Hidestate = false;
                    Player_obj.GetComponent<Rigidbody>().isKinematic=false;         //重力オン
                    Player_obj.GetComponent<BoxCollider>().enabled=true;            //コライダーつける
                    Player_obj.GetComponent<MeshRenderer>().enabled=true;           //プレイヤー見える
                    Parent_Player_obj.transform.position = Hide_before_pos;         //プレイヤーの位置を戻す
                }
            }
        }
    }
}
