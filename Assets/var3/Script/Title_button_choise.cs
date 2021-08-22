using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Title_button_choise : MonoBehaviour {
    public static int SelectPAndD = 0;
 
    // Use this for initialization
    void Start () {
         
    }
     
    // Update is called once per frame
    void Update () {
         
    }
 
    //ボタンを押した時の処理
    public void Click_Demon()
    {
        //鬼の時
        SelectPAndD = 1;
    }

    public void Click_warugi()
    {
        //プレイヤーの時
        SelectPAndD = 0;
    }
}