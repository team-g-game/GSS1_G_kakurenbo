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
    private string _play_ID;

    public string play_ID{get{return this._play_ID;}
        set
        {
            this._play_ID = value;
            Propeties_Hash_string("play_ID",_play_ID);
        }
    }
    private string _play_name;
    public string play_name {get{return this._play_name;}
        set
        {
            this._play_name = value;
            Propeties_Hash_string("name",_play_name);
        }
    }
    private int _haid_int;
    public int haid_int{get{return _haid_int;}
        set
        {
            this._haid_int = value;
            Propeties_Hash_int("haid_int",_haid_int);
        }
    }
    private bool _Catch = false;
    public bool Catch {get{return this._Catch;} 
        set
        {
            this._Catch = value;
            Propeties_Hash_bool("Catch",_Catch);
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
