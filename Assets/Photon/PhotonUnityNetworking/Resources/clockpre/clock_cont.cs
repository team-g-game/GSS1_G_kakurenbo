using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class clock_cont : MonoBehaviourPunCallbacks
{
    public static float num;
    public float max_num;
    public float add_num;
    float start_time;
    float end_time;
    bool stop = true;
    float bef_time;

    // Start is called before the first frame update
    void Start()
    {
        num = 0;
        bef_time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(stop){

        }
        else{
            add_num += Time.deltaTime;
            num = max_num - add_num;
            /*
            if(num > 0 && unchecked(PhotonNetwork.ServerTimestamp/1000 - end_time) > 0){
                add_num += (float)PhotonNetwork.ServerTimestamp/1000 - bef_time;
                num = max_num - add_num;
            }
            else{
                stop = true;
            }
            bef_time = (float)PhotonNetwork.ServerTimestamp/1000;
            */
        }
    }
    public string timer_start(string times){
        string ret_str = "正常に起動しました";
        start_time = int.Parse(times.Split(',')[0])/1000;
        end_time = int.Parse(times.Split(',')[1])/1000;

        bef_time = (float)PhotonNetwork.ServerTimestamp/1000;
        if((int)end_time <= bef_time){
            stop = true;
            ret_str = "時間が終了時間を過ぎています";
            
        }else{
            stop = false;
            max_num = end_time - start_time;
        }
        Debug.Log(ret_str);
        return ret_str;
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //データの送信
            stream.SendNext(num);
        }
        else
        {
            //データの受信
            num = (int)(float)stream.ReceiveNext();
        }
    }
}