using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Title_button_choise : MonoBehaviour {
 
    // Use this for initialization
    void Start () {
         
    }
     
    // Update is called once per frame
    void Update () {
         
    }
 
    //ボタンを押した時の処理
    public void Click_Demon()
    {
        int Demon;
        Demon = 1;
        //ログ出力
        Debug.Log(Demon);
    }

    public void Click_warugi()
    {
        int gaki;
        gaki = 0;
        //ログ出力
        Debug.Log(gaki);
    }
}