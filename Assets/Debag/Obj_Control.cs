using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Control : MonoBehaviour
{
    // public int loop_time;
    private float meta_time;
    GameObject main_camera;
    // Start is called before the first frame update
    void Start()
    {
        main_camera = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        meta_time += Time.deltaTime;
        if (meta_time >= 10){
            meta_time -= 10;
            foreach (var obj in GameObject.FindGameObjectsWithTag("bullt")){
                var pos = obj.transform.position;
                float x_dist = pos.x + main_camera.transform.position.x;
                float y_dist = pos.y + main_camera.transform.position.y;
                float z_dist = pos.z + main_camera.transform.position.z;
                if (psitive_num(x_dist) >= 100 || psitive_num(y_dist) >= 100 || psitive_num(z_dist) >= 100){
                    Destroy(obj);
                }
            }
        }
    }
    private float psitive_num(float a){
        float ans = 0;
        if (a < 0) {
            ans = a * -1;
        }else{
            ans = a;
        }
        return ans;
    }
}
