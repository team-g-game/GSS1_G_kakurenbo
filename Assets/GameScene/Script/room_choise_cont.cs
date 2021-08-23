using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon;
using Photon.Realtime;
public class room_choise_cont : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public Canvas title_canvas;
    public Text plyer_name ;
    public Text[] room_list;
    void Start()
    {
        Connect("v1.0");

    }
    private void Connect(string varsion){
        if(PhotonNetwork.IsConnected == false){
            Debug.Log("ロビーに入りました");
            PhotonNetwork.GameVersion = varsion;
            PhotonNetwork.ConnectUsingSettings();
            OnJoinedLobby();
        }
    }
    
    public void OnJoinedLobby(){
        for (int i = 1;i<=2; i++){
            RoomOptions roomoption = new RoomOptions(){
                MaxPlayers = 6,
                IsOpen = true,
                IsVisible = true,
                CustomRoomPropertiesForLobby = new string[]{"Stage", "Difficulty"}
            };
            Debug.Log("ルームを作りました" + i.ToString());
            PhotonNetwork.JoinOrCreateRoom("room"+i.ToString(),roomoption,null);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomlist){       
        foreach (var item in roomlist)
        {
            if(item.Name == "room1"){
                room_list[0].text = "参加人数　" + item.PlayerCount.ToString() + "/6";
            }
            if(item.Name == "room2"){
                room_list[0].text = "参加人数　" + item.PlayerCount.ToString() + "/6";
            }
        }
    }
    public void NEW_Down(){
    }
    public void RANDOM_Down(){
        if(PhotonNetwork.IsConnected){
            PhotonNetwork.LocalPlayer.NickName = plyer_name.text;
        }
        PhotonNetwork.JoinRandomRoom();        
    }
    public void REFRESH_Down(){

    }
    public void room_select_1(){
        room_conect("room1");
    }
    public void room_select_2(){
        room_conect("room2");
    }
    public void room_conect(string name){
        if(PhotonNetwork.IsConnected){
            PhotonNetwork.LocalPlayer.NickName = plyer_name.text;
        }
        PhotonNetwork.JoinRoom(name);
        
    }
}
