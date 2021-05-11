using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Cam_cnt : MonoBehaviour
{
    public float DPS;
    public bool move_flag = false;
    public Transform obj_tra;
    private Vector2 newAngle = new Vector2(0,0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = obj_tra.position;
        if (move_flag) {
            // マウスの移動量
            newAngle.y += Input.GetAxis("Mouse X") * DPS;
            newAngle.x -= Input.GetAxis("Mouse Y") * DPS;
            if (newAngle.x >= 85){
                newAngle.x = 84;
            }else if (newAngle.x <= -85){
                newAngle.x = -85;
            }

            this.gameObject.transform.localEulerAngles = newAngle;
        }
    }
}
