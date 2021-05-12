using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class maru_tes : MonoBehaviourPunCallbacks
{
    // Use this for initialization
    void Start () {
        //旧バージョンでは引数必須でしたが、PUN2では不要です。
        PhotonNetwork.ConnectUsingSettings();
    }

    void OnGUI()
    {
        //ログインの状態を画面上に出力
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }


    //ルームに入室前に呼び出される
    public override void OnConnectedToMaster() {
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }

    //ルームに入室後に呼び出される
    public override void OnJoinedRoom(){
        //キャラクターを生成
        GameObject monster = PhotonNetwork.Instantiate("Obj", Vector3.zero, Quaternion.identity, 0);
        move move_Script = monster.GetComponent<move>();

        move_Script.enabled = true;
    }
}
