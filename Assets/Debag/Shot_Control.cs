using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Control : MonoBehaviour
{
    public int Eva_time_miri;
    private float meta_time;
    private bool avoid_flag = false;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.layer = 8;
        avoid_flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (meta_time >= Eva_time_miri && avoid_flag){ 
            this.gameObject.layer = 0;
            avoid_flag = false;
        }
        else {
            meta_time += Time.deltaTime * 1000f;
        }
    }
}
