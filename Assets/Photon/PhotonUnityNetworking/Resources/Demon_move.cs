using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_move : MonoBehaviour
{
    public GameObject cam_obj;
    public GameObject ply_obj;
    public float move_speed;
    public int DPS;
    private Vector2 newAngle = new Vector2(0,0);

    // Start is called before the first frame update
    void Start()
    {
        var this_pos = transform.position;
        this_pos.y = 3f;
        ply_obj.transform.position = this_pos;
    }

    // Update is called once per frame
    void Update()
    {
        var this_pos = transform.position;

        if(Input.GetKey(KeyCode.W)) this_pos += ply_obj.transform.forward * Time.deltaTime * move_speed;
        if(Input.GetKey(KeyCode.A)) this_pos -= ply_obj.transform.right * Time.deltaTime * move_speed;
        if(Input.GetKey(KeyCode.D)) this_pos += ply_obj.transform.right * Time.deltaTime * move_speed;
        if(Input.GetKey(KeyCode.S)) this_pos -= ply_obj.transform.forward * Time.deltaTime * move_speed;
        
        transform.position = this_pos;
        var kyara = ply_obj.transform.localPosition;
        kyara.y = 0;
        transform.position -= kyara;
        
        kyara = ply_obj.transform.localPosition;
        kyara.z = 0;
        kyara.x = 0;        
        ply_obj.transform.localPosition = kyara;
        
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
        cam_obj.transform.localPosition = new Vector3(0,ply_obj.transform.position.y + 0.8f,0);
    }
    
}
