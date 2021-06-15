using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float move_speed;
    public float DPS;
    public GameObject Cam_Obj;
    public GameObject kyara_Obj;
    Vector3 targetPos;
    private Vector2 newAngle = new Vector2(0,0);
    // Start is called before the first frame update
    void Start()
    {
        var pos = kyara_Obj.transform.position;
        pos.y = 3;
        pos.x = 10;
        pos.z = 10;
        kyara_Obj.transform.position = pos;

        Camera cam_comp = Cam_Obj.GetComponent<Camera>();
        cam_comp.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {        

        var pos = transform.position; 

        if(Input.GetKey(KeyCode.W)) pos += kyara_Obj.transform.forward * Time.deltaTime * move_speed;
        if(Input.GetKey(KeyCode.S)) pos -= kyara_Obj.transform.forward * Time.deltaTime * move_speed;
        if(Input.GetKey(KeyCode.A)) pos -= kyara_Obj.transform.right * Time.deltaTime * move_speed;
        if(Input.GetKey(KeyCode.D)) pos += kyara_Obj.transform.right * Time.deltaTime * move_speed;

        if (kyara_Obj.transform.localPosition.x == Vector3.zero.x &&kyara_Obj.transform.localPosition.z == Vector3.zero.x){
            transform.position = pos;
        }else{
            var kyara = kyara_Obj.transform.position;
            kyara.y = 0;
            transform.position = kyara;

            kyara = kyara_Obj.transform.localPosition;
            kyara.z = 0;
            kyara.x = 0;        
            kyara_Obj.transform.localPosition = kyara;
        }        

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
        //OnDrawGizmos();
        kyara_Obj.transform.rotation = new Quaternion(0,0,0,0);
        var rot = Cam_Obj.transform.rotation;
        rot.x = 0;
        rot.z = 0;
        kyara_Obj.transform.rotation = rot;

    }
    /*
    void OnDrawGizmos(){
        Ray ry = new Ray (kyara_Obj.transform.position , (kyara_Obj.transform.forward * -1) + Cam_Obj.transform.position); 
        RaycastHit hit;
        if (Physics.SphereCast(kyara_Obj.transform.position,1f,Cam_Obj.transform.position,out hit,5f)){

            Gizmos.DrawRay(kyara_Obj.transform.position,kyara_Obj.transform.forward * -1 * hit.distance);
            Gizmos.DrawWireSphere((kyara_Obj.transform.position + Cam_Obj.transform.position)* hit.distance,1f);

            var pos = Cam_Obj.transform.localPosition;
            pos = kyara_Obj.transform.position + (kyara_Obj.transform.forward * -1 )* hit.distance;
            Cam_Obj.transform.localPosition = pos;
        }else{
            Gizmos.DrawRay(kyara_Obj.transform.position, Cam_Obj.transform.position);
            Gizmos.DrawWireSphere(kyara_Obj.transform.position + Cam_Obj.transform.position,1f);

            //Gizmos.DrawRay(kyara_Obj.transform.localPosition,kyara_Obj.transform.forward * -1 * hit.distance);
            //Gizmos.DrawWireSphere(kyara_Obj.transform.localPosition + (kyara_Obj.transform.forward * -1 )* hit.distance,1f);

            var pos = Cam_Obj.transform.localPosition;
            pos = -(Cam_Obj.transform.forward * 5f);
            Cam_Obj.transform.localPosition = pos;
        }
        Debug.Log(hit);
    }*/
}
