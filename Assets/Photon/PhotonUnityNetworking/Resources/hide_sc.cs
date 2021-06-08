using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hide_sc : MonoBehaviour
{
    // Start is called before the first frame update
    public bool HideTrriger = false;    //隠れることが可能な場所かどうか
    public bool WatchTrriger = false;   //見ることが可能な場所かどうか
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Play_1")
        {
            HideTrriger = true;
            this.GetComponent<MeshRenderer>().enabled=true;//メッシュレンダーの表示
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
            this.GetComponent<MeshRenderer>().enabled=false;//メッシュレンダーの表示
        }
    }
    void Start()
    {
        
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
            }
        }
    }
}
