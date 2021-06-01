using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit : MonoBehaviour
{
    // Start is called before the first frame update
    public bool HideTrriger = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Play_1")
        {
            HideTrriger = true;
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
