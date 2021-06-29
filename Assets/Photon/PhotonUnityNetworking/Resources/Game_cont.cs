using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Photon.Chat;

public class Game_cont : MonoBehaviourPunCallbacks
{
    ExitGames.Client.Photon.Hashtable roomHash;

    // Start is called before the first frame update
    void Start()
    {
        roomHash = new ExitGames.Client.Photon.Hashtable();
        PhotonNetwork.ConnectUsingSettings();

    }

    // Update is called once per frame
    public override void OnConnectedToMaster(){
       // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);

    }
    public override void OnJoinedRoom(){
        game_start_up();
        SetCustomProperties();
    }
    public void SetCustomProperties()
    {
        Hashtable i = new Hashtable();
        i.Add("aaaa",2);
        i.Add("rect",10);
        string[] str = new string[2];
        str = new string[]{"aaaa","rect"};
        // ハッシュに要素を追加(同じ名前があるとエラーになる)
        roomHash["uunnnn"] = str;

        // ハッシュに要素を追加、既に同じ名前のキーがあれば上書き
        //roomHash["hoge"] = 1;

        // ルームにハッシュを送信する
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable a){
        Debug.Log(a);
    }
    void Update()
    {

    }



    void game_start_up(){
        //逃げる側を動かす
        main_play_samon();
        //追いかける側を動かす
        //Demon_samon();
    }
    void main_play_samon(){
        GameObject players = PhotonNetwork.Instantiate("Play_1_obj",Vector3.zero,Quaternion.identity,0);
        move move_Script = players.GetComponent<move>();
        move_Script.enabled = true;
    }
    void Demon_samon(){
        GameObject players = PhotonNetwork.Instantiate("Demon",Vector3.zero,Quaternion.identity,0);
        Demon_move move_Script = players.GetComponent<Demon_move>();
        move_Script.enabled = true;
    }
}
