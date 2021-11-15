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
    public GameObject GameManager;  //Game_masterを入れる
    private Game_cont ScriptGameCont;   //Game_contの関数使えるようにする
    private bool start_flag= false ;
    private float chach_int = 2;
    private float _timer = 0;
    public AudioClip sound1;
    AudioSource audioSource;

    void Start()
    {
        var Col = this.GetComponent<SphereCollider>();
        Col.radius = rad;
        GameManager = GameObject.Find("Game_master");
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
        audioSource.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(Game_cont.Game_Status){
            case Game_cont.Status.before:{
                
                break;
            }
            case Game_cont.Status.play:{
                if(_timer >= chach_int)start_flag = true;
                else _timer += Time.deltaTime;
                break;
            }
            default:
                break;
        }
    }

    void OnTriggerStay(Collider hako)   //コライダーの中にいるとき呼び出される関数
    {
        //if (ScriptGameCont.DemonFlag == true && ScriptGameCont.CreatePlayerListFlag == true){ //鬼である、かつ、プレイヤーリスト作った後
        if(start_flag){
            if (hako.gameObject.CompareTag("Player"))   //コライダーのtagがPlayerであるとき
            {
                GameObject Target = hako.gameObject;
                var diff = Target.transform.position - demon_cam.transform.position;  //プレイヤーと鬼の距離を取得(Vector3)
                var distance = diff.magnitude;  //Vector3の大きさ
                var direction = diff.normalized;    //Vector3の向き
                var viewportPos = demon_cam.WorldToViewportPoint(Target.transform.position);
                var CameraToPlayer = demon_cam.transform.forward;
                        
                if (Vector3.Dot(CameraToPlayer,direction) >0){
                    if(rect.Contains(viewportPos) && GetComponent<PhotonView>().IsMine)//自分が鬼なら見つけたlogを出す
                    {
                        if(Physics.Raycast(demon_cam.transform.position, direction, out hit, distance))   //RaycastをPlayer方向に飛ばす
                        {
                            if(hit.transform.gameObject == Target)  //軌道上にPlayerがいるとき
                            {
                                
                                PhotonView View = hako.transform.parent.gameObject.GetComponent<PhotonView>();
                                string CViewId = View.ViewID.ToString();
                                if ((string)ScriptGameCont.GetPlayerInfo(CViewId, "CatchFlag") == "False"){
                                    int Index = ScriptGameCont.GetPlayerInfoIndexFromViewId(CViewId);
                                    ScriptGameCont.UpdatePlayerInfoListByIndex(Index, "CatchFlag", "True");
                                    ScriptGameCont.UpdatePlayerInfoListByIndex(Index, "HidePlace", "100");
                                    ScriptGameCont.SendPlayerInfo(Index);
                                    Debug.Log("見つけた");
                                    ScriptGameCont.CatchPlayerFlag = true;
                                    audioSource.PlayOneShot(sound1);    //捕まえたときに鳴らす音

                                }
                                else {
                                    Debug.Log("nannkaokasii");
                                }
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
                else{
                    Debug.Log("画面外後ろ側");
                }
            }
        }
    }
}
