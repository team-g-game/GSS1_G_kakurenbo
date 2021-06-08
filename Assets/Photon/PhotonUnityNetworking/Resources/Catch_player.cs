using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch_player : MonoBehaviour
{
    // Start is called before the first frame update
    private RaycastHit hit; //Raycastの情報を取得するための構造体
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider hako)   //コライダーの中にいるとき呼び出される関数
    {
        if (hako.gameObject.CompareTag("Player"))   //コライダーのtagがPlayerであるとき
        {
            GameObject Target = GameObject.Find($"{hako.name}");    //Playerの名前を取得
            var diff = Target.transform.position - transform.position;  //プレイヤーと鬼の距離を取得(Vector3)
            var distance = diff.magnitude;  //Vector3の大きさ
            var direction = diff.normalized;    //Vector3の向き

            if(Physics.Raycast(transform.position, direction, out hit, distance))   //RaycastをPlayer方向に飛ばす
            {
                if(hit.transform.gameObject == Target)  //軌道上にPlayerがいるとき
                {
                    Debug.Log("見つけた");
                }
                else    //いないとき
                {
                    Debug.Log("見つけてない");
                }

            }
        }
    }
}
