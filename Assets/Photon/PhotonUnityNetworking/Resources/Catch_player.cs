using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Catch_player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera demon_cam;
    [SerializeField] private int rad = 20;
    private RaycastHit hit; //Raycastの情報を取得するための構造体
    Rect rect = new Rect(0, 0, 1, 1);
    void Start()
    {
        var Col = this.GetComponent<SphereCollider>();
        Col.radius = rad;
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
            var diff = Target.transform.position - demon_cam.transform.position;  //プレイヤーと鬼の距離を取得(Vector3)
            var distance = diff.magnitude;  //Vector3の大きさ
            var direction = diff.normalized;    //Vector3の向き
            var viewportPos = demon_cam.WorldToViewportPoint(Target.transform.position);

            if(rect.Contains(viewportPos) && GetComponent<PhotonView>().IsMine)//自分が鬼なら見つけたlogを出す
            {
                if(Physics.Raycast(demon_cam.transform.position, direction, out hit, distance))   //RaycastをPlayer方向に飛ばす
                {
                    if(hit.transform.gameObject == Target)  //軌道上にPlayerがいるとき
                    {
                        Debug.Log("見つけた");
                    }
                    else{
                        Debug.Log("見つけてない");
                    }                         
                }
            }
            else{
                Debug.Log("画面外だよ");
            }

        }
    }
}
