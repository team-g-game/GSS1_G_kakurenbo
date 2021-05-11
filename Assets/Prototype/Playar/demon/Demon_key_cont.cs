using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_key_cont : MonoBehaviour
{
    public bool key_contlol = false;
    public float move_speed;
    public Transform Com_Obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        transform.rotation = new Quaternion(0,0,0,0);
        */
        var rot = Com_Obj.rotation;
        rot.x = 0;
        rot.z = 0;
        transform.rotation = rot;
        if(key_contlol){
            var pos = key_move(transform.position);
            transform.position = pos;
        } 
    }
    public Vector3 key_move(Vector3 pos)
    {
        if(Input.GetKey(KeyCode.W)) pos += transform.forward * Time.deltaTime * move_speed;
        if(Input.GetKey(KeyCode.S)) pos -= transform.forward * Time.deltaTime * move_speed;
        if(Input.GetKey(KeyCode.A)) pos -= transform.right * Time.deltaTime * move_speed;
        if(Input.GetKey(KeyCode.D)) pos += transform.right * Time.deltaTime * move_speed;
        if(Input.GetKey(KeyCode.Space)) pos.y = move_speed;
        return pos;
    }
}
