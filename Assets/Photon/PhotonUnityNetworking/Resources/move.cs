using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using Photon.Realtime;
public class move : MonoBehaviour
{
    public Vector3 start_pos;
    public float move_speed;
    public float DPS;
    public GameObject Cam_Obj;
    public GameObject kyara_Obj;
    Vector3 targetPos;
    private Vector2 newAngle = new Vector2(0,0);
    private PhotonView view = null;
    public string playe_id;
    public static int MyPlayerViewId = 0;　//自分のViewIdを入れる
    public static List<int> PlayerViewIdsList = new List<int>();    //プレイヤー全員のViewIdが入る
    public int MovePlayerViewId;
    private bool CheckMovePlayerViewId = false;
    public GameObject GameManager;  //Game_masterを入れる
    private Game_cont ScriptGameCont;   //Game_contの関数使えるようにする
    private bool VisualFlag = true;
    private List<Vector3> StartPosList = new List<Vector3>();   //スタート位置一覧
    private Vector3 StartPosition;  //プレイヤーのスタート位置が入る
    public static bool SetToStartPositionFlag = false;  //スタートの位置にプレイヤーを設定したかどうか

    public List<GameObject> camera_chac = new List<GameObject>();
    private int play_num = 0;
    public GameObject sinndaato;


    void Awake(){
        view = GetComponent<PhotonView>();
        
        if(view.IsMine){
            
            playe_id = PhotonNetwork.LocalPlayer.UserId;
            //Debug.Log(playe_id);
            
            Camera cam_comp = Cam_Obj.GetComponent<Camera>();
            AudioListener cam_lis = Cam_Obj.GetComponent<AudioListener>();
            cam_lis.enabled = true;
            cam_comp.enabled = true; 
            MyPlayerViewId = view.ViewID;
            PlayerViewIdsList.Add(view.ViewID);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        kyara_Obj.transform.position = start_pos;
        GameManager = GameObject.Find("Game_master");
        ScriptGameCont = GameManager.GetComponent<Game_cont>();
        StartPosSetting();
    }


    // Update is called once per frame
    void Update()
    {        

        var pos = transform.position; 

        if (view.IsMine && !hide_sc.Hidestate){
            if (Game_cont.GameStartFlag == true){
                if (ScriptGameCont.GetPlayerInfo(MyPlayerViewId.ToString(), "CatchFlag") == "True"){     
                    sinndaato.SetActive(true);
                    if(camera_chac.Count == 0){
                        foreach(GameObject i in GameObject.FindGameObjectsWithTag("player")){
                            if(i.GetComponent<PhotonView>().ViewID != MyPlayerViewId)camera_chac.Add(i);
                    }
                }
            }
                else {
                    if(Input.GetKey(KeyCode.W)) pos += kyara_Obj.transform.forward * Time.deltaTime * move_speed;
                    if(Input.GetKey(KeyCode.S)) pos -= kyara_Obj.transform.forward * Time.deltaTime * move_speed;
                    if(Input.GetKey(KeyCode.A)) pos -= kyara_Obj.transform.right * Time.deltaTime * move_speed;
                    if(Input.GetKey(KeyCode.D)) pos += kyara_Obj.transform.right * Time.deltaTime * move_speed;            
                }                 
            }
        }




        if (kyara_Obj.transform.localPosition.x == Vector3.zero.x &&kyara_Obj.transform.localPosition.z == Vector3.zero.x && view.IsMine){
            transform.position = pos;
        }else{
            var kyara = kyara_Obj.transform.position;
            kyara.y = 0;
            transform.position = kyara;

            kyara = kyara_Obj.transform.localPosition;
            if (kyara.y < 0.9){
                kyara.y = 1;
            }
            kyara.z = 0;
            kyara.x = 0;        
            kyara_Obj.transform.localPosition = kyara;
        }        
        
        if(view.IsMine && ScriptGameCont.GetPlayerInfo(MyPlayerViewId.ToString(), "CatchFlag") != "True"){
            targetPos = kyara_Obj.transform.position;

            // マウスの移動量
            newAngle.x = Input.GetAxis("Mouse X") * Time.deltaTime * DPS;
            newAngle.y = Input.GetAxis("Mouse Y") * Time.deltaTime * DPS;
            
            if (newAngle.x >= 85){
                newAngle.x = 84;
            }else if (newAngle.x <= -85){
                newAngle.x = -85;
            }
            // targetの位置のY軸を中心に、回転（公転）する
            Cam_Obj.transform.RotateAround(targetPos, Vector3.up, newAngle.x);
            // カメラの垂直移動（※角度制限なし、必要が無ければコメントアウト）
            Cam_Obj.transform.RotateAround(targetPos, Cam_Obj.transform.right, newAngle.y);
                    
            var diff = kyara_Obj.transform.position - Cam_Obj.transform.position;  //プレイヤーとカメラの距離を取得(Vector3)
            var distance = diff.magnitude;  //Vector3の大きさ
            var direction = -(diff.normalized);    //Vector3の向き
            RaycastHit hit; //Raycastの情報を取得するための構造体
            if(direction.y > 0.9f){
                direction.y = 0.9f;
            }else if (direction.y < -0.7f){
                direction.y = -0.7f;
            }
            if(Physics.Raycast(kyara_Obj.transform.position, direction, out hit, distance))   //RaycastをPlayer方向に飛ばす
            {
                pos = kyara_Obj.transform.position + direction * hit.distance;
                Cam_Obj.transform.position = pos;
            }else{
                var a = kyara_Obj.transform.position + direction * 5f;
                
                Cam_Obj.transform.position = a;
            }
            Cam_Obj.transform.LookAt(kyara_Obj.transform);

            //Debug.Log(direction);
            

            kyara_Obj.transform.rotation = new Quaternion(0,0,0,0);
            var rot = Cam_Obj.transform.rotation;
            rot.x = 0;
            rot.z = 0;
            kyara_Obj.transform.rotation = rot;            
        }else{
            Cam_Obj.GetComponent<Camera>().enabled = false;
            foreach(var _ in camera_chac)_.GetComponent<Camera>().enabled =false;
            camera_chac[play_num].GetComponent<Camera>().enabled = true;
        }


        if (Game_cont.JoinRoomFlag == true){
            CreatePlayerIdsList();
        }
        if (CheckMovePlayerViewId == false){
            MovePlayerViewId = GetComponent<PhotonView>().ViewID;
        }
        if (Game_cont.CreatePlayerListFlag == true){
            UpdateCharacterVisualTrriger();
        }
        SetToStartPosition();
    }

    /// <summary>
    /// プレイヤーのViewIdリストを作成。
    /// </summary>
    void CreatePlayerIdsList(){
        view = GetComponent<PhotonView>();
        if (!(PlayerViewIdsList.Contains(view.ViewID))){
            PlayerViewIdsList.Add(view.ViewID);
        }
    }
    
    /// <summary>
    /// 送られてきたハッシュで隠れている場合に、プレイヤーを表示しないようにする
    /// </summary>
    void UpdateCharacterVisualTrriger(){
        int HideP = int.Parse(ScriptGameCont.GetPlayerInfo(MovePlayerViewId.ToString(), "HidePlace"));
        if (HideP == 0){
            if (VisualFlag == true){
                hide_sc.VisualTrriger(kyara_Obj, true);
                VisualFlag = false;
            }
        }
        else if (HideP == 100){
            hide_sc.VisualTrriger(kyara_Obj, false);
            kyara_Obj.GetComponent<MeshRenderer>().enabled=true;           //プレイヤー見える
        }
        else if (HideP < 100 && HideP > 0){
            if (VisualFlag == false){
                hide_sc.VisualTrriger(kyara_Obj, false);
                VisualFlag = true;
            }
        }
    }

    void StartPosSetting(){
        StartPosList.Add(new Vector3(-90, 12, 94));
        StartPosList.Add(new Vector3(95, 12, 95));
        StartPosList.Add(new Vector3(110, 12, -122));
        StartPosList.Add(new Vector3(-128, 12, -126));
        StartPosList.Add(new Vector3(46, 12, -117));
    }

    void SetToStartPosition(){
        if (Game_cont.CreatePlayerListFlag == true){
            PlayerViewIdsList.Sort();
            for (int i = 0; i < PlayerViewIdsList.Count; ++i){
                if(MyPlayerViewId == PlayerViewIdsList[i]){
                    StartPosition = StartPosList[i];
                }
            }
        }
        if(view.IsMine == true){
            if (Game_cont.GameStartFlag == true){
                if (SetToStartPositionFlag == false){
                    kyara_Obj.transform.position = StartPosition;
                    SetToStartPositionFlag = true;
                }
            }            
        }
    }
    public void left_click(){
        play_num ++;
        if(camera_chac.Count < play_num) play_num = 0;
    }
    public void Rifht_Click(){
        play_num --;
        if(play_num < 0) play_num = 0;
    }
}
