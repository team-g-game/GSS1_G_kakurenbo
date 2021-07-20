using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Photon.Chat;

public class Game_cont : MonoBehaviourPunCallbacks
{
    ExitGames.Client.Photon.Hashtable roomHash;
    private Dictionary<string,haid_player_propateli> _Players = new Dictionary<string, haid_player_propateli>();
    public Dictionary<string,haid_player_propateli> players{
        get
        {
            return this._Players;
        }
        set
        {
            string keay = "";
            haid_player_propateli hpp = null;
            foreach(var v in value){
                if(_Players[v.Key] != v.Value){
                    keay = v.Key;
                    hpp = v.Value;
                    break;
                }
            }
            Debug.Log(keay + hpp + "aaaa");
            _Players = value;
            if(keay ==""&&hpp ==null){
                propeties_hash_HPP(keay,hpp);
            }
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
        string id =PhotonNetwork.LocalPlayer.UserId;
        haid_player_propateli haid_a = new haid_player_propateli("自分",0,false);
        players.Add(id,haid_a) ;
        Debug.Log(players.Keys + players.Values.ToString());
        Propeties_Hash_string("player_id",PhotonNetwork.LocalPlayer.UserId);
        
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable a){
        if(a["player_id"] != null){
            Debug.Log("player_id実行しました");
            if(players.Keys !=null){
                List<string> key_lis = new List<string>(players.Keys);
                if(key_lis.Contains(a["player_id"].ToString()) == false){
                    players.Add(a.ToString(),new haid_player_propateli());
                    Debug.Log(players);
                } 
            }
        }
        Debug.Log("kkk" + a[PhotonNetwork.LocalPlayer.UserId]);
        
        foreach(var v in players){
            if(a[v.Key] != null){
                Debug.Log(a[v.Key].ToString() + "aaaaaa");
                players[v.Key] = (haid_player_propateli)a[v.Key];
            }
        }
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
    public void propeties_hash_HPP (string name ,haid_player_propateli hpp){
        roomHash[name] = hpp;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
        Debug.Log("hppハッシュを飛ばしました");
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
    public class haid_player_propateli
    {
        public string play_name;
        public int haid_int;
        public bool Catch ;
        public haid_player_propateli(string N = "仮",int haid =0,bool cat = false){
            play_name = N;
            haid_int = haid;
            Catch = cat;
        }
    }
}
