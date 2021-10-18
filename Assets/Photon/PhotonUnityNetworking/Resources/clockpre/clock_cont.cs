using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class clock_cont : MonoBehaviour
{
    /*
    		clock_cont.num = 100;
		clock_cont.stop = false;
		clock_cont.clock_stat = clock_cont.stat.DOWN;
		*/
    public static float num;
    public static stat clock_stat;
    public enum stat
    {
        DOWN = 0,
        UP = 1
    }
    public static bool stop;
    float bef_time;

    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        clock_stat = stat.DOWN;
        num = 0;
        bef_time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(stop){

        }
        else{
            switch(clock_stat){
                case stat.DOWN:{
                    num -= (float)PhotonNetwork.Time - bef_time;
                    bef_time =  (float)PhotonNetwork.Time;
                    break;
                }
                case stat.UP:{
                    num += (float)PhotonNetwork.Time - bef_time;
                    bef_time =  (float)PhotonNetwork.Time;
                    break;
                }
            }
        }

    }
}
