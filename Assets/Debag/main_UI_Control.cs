using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class main_UI_Control : MonoBehaviour
{
    public int FPS_Reload_millisec;
    private float FPS;
    private int Before_FPS;
    private Text FPS_text;
    private float meta_time;
    
    // Start is called before the first frame update
    void Start()
    {
        FPS_text = this.transform.Find("FPS_UI").GetComponent<Text>();
        FPS = 1f / Time.deltaTime;
        Before_FPS = (int)FPS;
        FPS_text.text = "FPS:" + Before_FPS.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        meta_time += 1000f * Time.deltaTime;
        if (meta_time >= FPS_Reload_millisec){
            meta_time = 0;
            FPS = 1f / Time.deltaTime;
            if (Before_FPS != (int)FPS){
                Before_FPS = (int)FPS;
                FPS_text.text = "FPS:" + Before_FPS.ToString();
            }

        }
    }
}
