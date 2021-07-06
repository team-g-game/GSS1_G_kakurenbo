using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Photon.Chat;

public class Game_cont : MonoBehaviourPunCallbacks
{
    ExitGames.Client.Photon.Hashtable roomHash;

    public List<string> playu_list;

    public string play_ID{get{return this.play_ID;}
        set
        {
            this.play_ID = value;
            Propeties_Hash_string(play_ID,value);
        }
    }
    public string play_name {get{return this.play_name;}
        set
        {
            this.play_name = value;
            Propeties_Hash_string(play_ID,value);
        }
    }
    public int haid_int{get{return haid_int;}
        set
        {
            this.haid_int = value;
            Propeties_Hash_int(play_ID,value);
        }
    }
    public bool Chat {get{return this.Chat;} 
        set
        {
            this.Chat = value;
            Propeties_Hash_bool(play_ID,value);
        }
    }
    void Start()
    {
        roomHash = new ExitGames.Client.Photon.Hashtable();
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster(){
       // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }
    public override void OnJoinedRoom(){
        game_start_up();
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable a){
        string b = a.ToString() + ":" + a.Values.ToString();
        Debug.Log(b);

    }
    void Update()
    {

    }
    public void Propeties_Hash_string(string name,string value)
    {
        roomHash[name] = value;
        // ルームにハッシュを送信する   
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
    }
        public void Propeties_Hash_int(string name,int value)
    {
        roomHash[name] = value;
        // ルームにハッシュを送信する        
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);        
    }
    public void Propeties_Hash_bool(string name,bool value)
    {
        roomHash[name] = value;
        // ルームにハッシュを送信する        
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);        
    }
    void game_start_up(){
        //逃げる側を動かす
        //main_play_samon();
        //追いかける側を動かす
        Demon_samon();
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
