using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_move : MonoBehaviour
{

    public float movespeed;
    public bool Shift_chack;
    public float Shift_Speed;
    public float DPS;
    public GameObject Shot_obj;
    public float Shot_Speed;
    public int Shot_intarval_meta;
    private float Shot_intarval;
    private Vector2 newAngle = new Vector2(0,0);
    // Start is called before the first frame update
    void Start()
    {
        Shot_intarval = Shot_intarval_meta;
    }

    // Update is called once per frame
    void Update()
    {
        Shot_intarval -= 1000f * Time.deltaTime;
        key_move();
        if (Cursor.visible == false) mous_move();

        if (Input.GetKeyUp(KeyCode.L)){
            if (this.GetComponent<Light>().enabled){
                this.GetComponent<Light>().enabled = false;
            }else{
                this.GetComponent<Light>().enabled = this;
            }
        }
        if (Input.GetKey(KeyCode.Space)){
            Fly_forward();
        }
    }
    void Fly_forward()
    {
        if (Shot_intarval <= 0){
            Shot_intarval = Shot_intarval_meta;
            GameObject fly_obj = Instantiate(Shot_obj,transform.position,transform.rotation);
            Rigidbody obj_rig = fly_obj.GetComponent<Rigidbody>();
            obj_rig.AddForce(transform.forward * Shot_Speed,ForceMode.Impulse);
        }
    }
    void key_move()
    {
        var for_move = transform.forward;
        var rig_move = transform.right;
        var pos = transform.position;

        if (Shift_chack && Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)){
            for_move = transform.forward * (movespeed + Shift_Speed) * Time.deltaTime;
            rig_move = transform.right * (movespeed + Shift_Speed) * Time.deltaTime; 
        }else{
            for_move = transform.forward * movespeed * Time.deltaTime;
            rig_move = transform.right * movespeed * Time.deltaTime; 
        }

        if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow)) pos += for_move;
        if(Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow)) pos -= for_move;
        if(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow)) pos += rig_move;
        if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow)) pos -= rig_move;
        
        transform.position = pos;
    }
    void mous_move()
    {
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
