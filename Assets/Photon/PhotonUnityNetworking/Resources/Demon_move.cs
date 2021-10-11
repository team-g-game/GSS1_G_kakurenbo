using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Demon_move : MonoBehaviour
{
    public Vector3 start_pos;
    public GameObject cam_obj;
    public GameObject ply_obj;
    public float move_speed;
    public int DPS;
    private Vector2 newAngle = new Vector2(0,0);

    private PhotonView view = null;

    public string Player_name;
    public string Player_id;

    public Canvas main_canvas;
    void Awake(){
        view = GetComponent<PhotonView>();
        main_canvas.enabled = view.IsMine;
        if(view.IsMine){
            Camera cam_comp = cam_obj.GetComponent<Camera>();
            AudioListener cam_lis = cam_obj.GetComponent<AudioListener>();
            Catch_player ct_ple = this.GetComponent<Catch_player>();
            ct_ple.enabled = true;
            cam_lis.enabled =true;
            cam_comp.enabled = true;                    
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        ply_obj.transform.position = start_pos;


    }

    // Update is called once per frame
    void Update()
    {
        var this_pos = transform.position;

        

        if (view.IsMine){
            if (Game_cont.GameStartFlag == true){
                if (Game_cont.GameEndFlag == false){
                    if(Input.GetKey(KeyCode.W)) this_pos += ply_obj.transform.forward * Time.deltaTime * move_speed;
                    if(Input.GetKey(KeyCode.A)) this_pos -= ply_obj.transform.right * Time.deltaTime * move_speed;
                    if(Input.GetKey(KeyCode.D)) this_pos += ply_obj.transform.right * Time.deltaTime * move_speed;
                    if(Input.GetKey(KeyCode.S)) this_pos -= ply_obj.transform.forward * Time.deltaTime * move_speed;
                }
            }
        }
        
        if (ply_obj.transform.localPosition.x == Vector3.zero.x &&ply_obj.transform.localPosition.z == Vector3.zero.x && view.IsMine){
            transform.position = this_pos;
        }else{
            var kyara = ply_obj.transform.position;
            kyara.y = 0;
            transform.position = kyara;

            kyara = ply_obj.transform.localPosition;
            kyara.z = 0;
            kyara.x = 0;        
            ply_obj.transform.localPosition = kyara;
        }

        // マウスの移動量
        newAngle.y += Input.GetAxis("Mouse X") * DPS;
        newAngle.x -= Input.GetAxis("Mouse Y") * DPS;
        if (newAngle.x >= 85){
            newAngle.x = 84;
        }else if (newAngle.x <= -85){
            newAngle.x = -85;
        }
        cam_obj.transform.localEulerAngles = newAngle;

        var this_rot = cam_obj.transform.rotation;
        this_rot.x = 0;
        this_rot.z = 0;
        ply_obj.transform.rotation = this_rot;
        /*
        this_rot = cam_obj.transform.rotation;
        this_rot.z = 0;
        cam_obj.transform.rotation = this_rot;
        */
        cam_obj.transform.localPosition = new Vector3(0,ply_obj.transform.position.y + 0.8f,0);
    }
    private Vector3 move_key_Down(KeyCode get_key){
        Vector3 move_pos = new Vector3(0,0,0);
        switch(get_key){
            case KeyCode.W:

                break;
            case KeyCode.A:

                break;            

            case KeyCode.S:

                break;

            case KeyCode.D:

                break;

            default:
                break;
        }
        return move_pos;
    }
}
