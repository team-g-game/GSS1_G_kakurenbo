using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class script_Test : MonoBehaviour
{
    ExitGames.Client.Photon.Hashtable roomHash;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // ローカルで使っているハッシュをルームにセット
    public void SetRoomProperty()
    {
        // ハッシュに要素を追加(同じ名前があるとエラーになる)
        roomHash.Add("hoge", 0);

        // ハッシュに要素を追加、既に同じ名前のキーがあれば上書き
        roomHash["hoge"] = 1;

        // ルームにハッシュを送信する
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable changedRoomHash){
        roomHash = changedRoomHash;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
