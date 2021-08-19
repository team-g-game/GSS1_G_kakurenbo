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
    public struct PlayerInfo{
        public int PViewId;
        public int PHidePlace;
        public bool PCatchFlag;

        public PlayerInfo(int PlayerViewId, int HidePlace, bool CatchFlag){
            PViewId = PlayerViewId;
            PHidePlace = HidePlace;
            PCatchFlag = CatchFlag;
        }
    }
    private bool CheckCreatId = false;  //一回だけ自分の情報を作って送るためにある
    PlayerInfo Myself;  //入った本人の情報
    PlayerInfo Player2; //ほかのプレイヤーの情報を作るために作ったけど要らない気がしてきた
    List<PlayerInfo> PlayerInfoList = new List<PlayerInfo>();   //プレイヤーの全情報が入ってる
    public static bool JoinRoomFlag = false;    //ルームに参加したタイミングを判定
    public static bool DemonFlag = false;       //鬼側のフラグ

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
        JoinRoomFlag = true;
        
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable a){
        if(a["player_id"] != null){
            //Debug.Log("player_id実行しました");
            if(players.Keys !=null){
                List<string> key_lis = new List<string>(players.Keys);
                if(key_lis.Contains(a["player_id"].ToString()) == false){
                    players.Add(a.ToString(),new haid_player_propateli());
                    Debug.Log(players);
                } 
            }
        }
        //Debug.Log("kkk" + a[PhotonNetwork.LocalPlayer.UserId]);
        
        foreach(var v in players){
            if(a[v.Key] != null){
                Debug.Log(a[v.Key].ToString() + "aaaaaa");
                players[v.Key] = (haid_player_propateli)a[v.Key];
            }
        }

        for (int i = 0; i < PlayerInfoList.Count; ++i){
            string value = (string)a[PlayerInfoList[i].PViewId.ToString()];
            if (value == null){ //これないとエラー出るから気を付けて
                Debug.Log("nullなんよそれ");
                break;
            }
            if (value == CreatePlayerValue(i)){
                Debug.Log("同じやつ来てる");
            }
            else if (value != CreatePlayerValue(i)){
                UpdatePlayerInfoList(i, value);
                Debug.Log(CreatePlayerValue(i) + "更新した");
            }
            Debug.Log("何回");
        }
    }
    void Update()
    {
        CheckMyViewId();
        if (Input.GetKeyDown(KeyCode.F)){   //これはテスト
            SendPlayerInfo(0);
        }
        if (Input.GetKeyDown(KeyCode.C)){   //プレイヤーのリストを作るタイミングで押す
            CreatePlayerList();
        }
        if (Input.GetKeyDown(KeyCode.G)){   //これもテスト
            Debug.Log(CreatePlayerValue(0) + CreatePlayerValue(1));
        }
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
        DemonFlag = true;
    }


    /// <summary>
    /// 自分の情報を作ってPlayerInfoListに入れて、オンラインにハッシュを送る
    /// </summary>
    void CheckMyViewId(){ 
        if (CheckCreatId == false && DemonFlag == false){
            if (move.MyPlayerViewId != 0){
                CheckCreatId = true;
                Myself = new PlayerInfo(move.MyPlayerViewId, 0, false);
                PlayerInfoList.Add(Myself);
                SendPlayerInfo(0);
            }
        }
    }

    /// <summary>
    /// PlayerInfoListの指定したプレイヤーの情報をオンラインハッシュに送る
    /// </summary>
    /// <param name="PlayerNum">プレイヤー番号</param>
    public void SendPlayerInfo(int PlayerNum){
        string PlayerId = CreatePlayerKey(PlayerNum);
        string SendValue = CreatePlayerValue(PlayerNum);
        Propeties_Hash_string(PlayerId,SendValue);
    }


    /// <summary>
    /// プレイヤーリストの中の構造体のデータを変更し、オンラインハッシュを送信する。
    /// 変えたい要素、HidePlace,CatchFlag
    /// </summary>
    /// <param name="PlayerNum">プレイヤー番号</param>
    /// <param name="ChangeElement">変えたい要素</param>
    /// <param name="ChangeContent">変わったあとの内容</param>
    public void ChangePlayerList(int PlayerNum, string ChangeElement, string ChangeContent){   //プレイヤー番号、変えたい要素、変わったあとの内容
        var example = PlayerInfoList[PlayerNum];
        if (ChangeElement == "CatchFlag"){
            example.PCatchFlag = System.Convert.ToBoolean(ChangeContent);
        }
        else if(ChangeElement == "HidePlace"){
            example.PHidePlace = int.Parse(ChangeContent);
        }
        else {
            Debug.Log("naiyo" + PlayerNum);
        }
        PlayerInfoList[PlayerNum] = example;
    }

    /// <summary>
    /// 送られてきたハッシュのValueでプレイヤー情報を更新する
    /// </summary>
    /// <param name="PlayerNum">プレイヤー番号</param>
    /// <param name="SentValue">送られてきたハッシュの一人のプレイヤーのバリュー</param>
    void UpdatePlayerInfoList(int PlayerNum,string SentValue){
        string[] part = SentValue.Split(',');
        ChangePlayerList(PlayerNum, "HidePlace", part[0]);
        ChangePlayerList(PlayerNum, "CatchFlag", part[1]);
    }

    /// <summary>
    /// 全プレイヤーの情報リストを作成
    /// </summary>
    void CreatePlayerList(){
        for (int i = 1; i < move.PlayerViewIdsList.Count; ++i){
            Player2 = new PlayerInfo(move.PlayerViewIdsList[i], 0, false);
            PlayerInfoList.Add(Player2);
        }
    }

    /// <summary>
    /// ハッシュに送るときのKeyをPlayerInfoListから作成
    /// </summary>
    /// <param name="PlayerNum">プレイヤー番号</param>
    /// <returns></returns>
    string CreatePlayerKey(int PlayerNum){
        string PlayerIdKey = PlayerInfoList[PlayerNum].PViewId.ToString();
        return PlayerIdKey;
    }

    /// <summary>
    /// ハッシュに送るときのValueをPlayerInfoLIstから作成
    /// </summary>
    /// <param name="PlayerNum">プレイヤー番号</param>
    /// <returns></returns>
    string CreatePlayerValue(int PlayerNum){
        string PlayerValue = "";
        PlayerValue += PlayerInfoList[PlayerNum].PHidePlace + ",";
        PlayerValue += PlayerInfoList[PlayerNum].PCatchFlag;
        return PlayerValue;
    }

    /// <summary>
    /// プレイヤー情報をViewIdで指定して、持ってくる。持ってきたい要素、HidePlace,CatchFlag
    /// </summary>
    /// <param name="PlayerViewId">持ってきたいプレイヤーのViewId</param>
    /// <param name="GetElement">持ってきたい要素</param>
    /// <returns></returns>
    public string GetPlayerInfo(string PlayerViewId, string GetElement){
        string Content = "";
        for (int i = 0; i < PlayerInfoList.Count; ++i){
            if (PlayerInfoList[i].PViewId.ToString() == PlayerViewId){
                if (GetElement == "CatchFlag"){
                    Content = PlayerInfoList[0].PCatchFlag.ToString();
                }
                else if(GetElement == "HidePlace"){
                    Content = PlayerInfoList[0].PHidePlace.ToString();
                }
                else {
                    Debug.Log("naiyo" + PlayerViewId);
                }
            }
        }
        return Content;
    }

    /// <summary>
    /// プレイヤーのViewIdをから、PlayerInfoListのIndexを返す。
    /// </summary>
    /// <param name="PlayerViewId"></param>
    /// <returns></returns>
    public int GetPlayerInfoIndex(string PlayerViewId){
        int Index = 0;
        for (int i = 0; i < PlayerInfoList.Count; ++i){
            if (PlayerViewId == PlayerInfoList[i].PViewId.ToString()){
                Index = i;
            }
        }
        return Index;
    }

    /// <summary>
    /// string型のハッシュをオンラインに送信する
    /// </summary>
    /// <param name="name">キーの名前</param>
    /// <param name="value">バリューの値</param>
    public void Propeties_Hash_string(string name,string value)
    {
        if ((string)roomHash[name] == value){
            Debug.Log("同じの送ってる");
        }
        else{
            roomHash[name] = value;
            // ルームにハッシュを送信する   
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
        }
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
