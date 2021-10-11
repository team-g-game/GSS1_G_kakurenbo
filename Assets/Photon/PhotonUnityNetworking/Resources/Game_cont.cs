using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Photon.Chat;

public class Game_cont : MonoBehaviourPunCallbacks
{
    ExitGames.Client.Photon.Hashtable roomHash;
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
    List<string> TreasureChest = new List<string>();
    public static bool JoinRoomFlag;    //ルームに参加したタイミングを判定
    public bool DemonFlag;       //鬼側のフラグ
    public static bool CreatePlayerListFlag;    //プレイヤーリストを生成したタイミングを判定
    public int DemonJoinedTime = 0;     //鬼がルームに入ってきた時間
    public static bool GameStartFlag;   //ゲームスタートタイミング
    public int CurrentTime;             //ルームの現在時刻
    public static bool GameEndFlag;     //ゲーム終了フラグ
    private bool StartCount = false;    //CreatPlayerListを一回だけ実行するためにある
    public static bool DemonJoinedFlag; //鬼が入ってきたかどうか判定
    public static bool DemonCatchStartFlag; //鬼が捕まえた判定をスタートする
    public static int decision;// 0なら鬼の勝ち
    public bool CatchPlayerFlag = false;
    public bool CreateTreasureChestListflag = false;
    public static Status Game_Statue;
    //ゲームの状態
    public enum Status 
    {
        before = 0,
        play = 1,
        after = 2
    }


    void Start()
    {
        JoinRoomFlag = false;
        DemonFlag = false;
        CreatePlayerListFlag = false;
        GameStartFlag = false;
        GameEndFlag = false;
        DemonJoinedFlag = false;
        DemonCatchStartFlag = false;
        decision = 0;
        Game_Statue = Status.before;
        
        roomHash = new ExitGames.Client.Photon.Hashtable();
        PhotonNetwork.ConnectUsingSettings();
        CreateTreasureChestList();
    }
    public override void OnConnectedToMaster(){
       // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）


        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);

    }
    public override void OnJoinedRoom(){
        game_start_up();
        string id =PhotonNetwork.LocalPlayer.UserId;
        Propeties_Hash_string("player_id",PhotonNetwork.LocalPlayer.UserId);
        JoinRoomFlag = true;
        if (DemonFlag == true){
            DemonJoinedTime = PhotonNetwork.ServerTimestamp;
            Propeties_Hash_string("DemonJoinedTime", DemonJoinedTime.ToString());
        }
        if (DemonJoinedTime == 0){
            object DemonTime = GetRoomProperty("DemonJoinedTime");
            if (DemonTime == null){}
            else {
                DemonJoinedTime = int.Parse((string)DemonTime);
                Debug.Log(DemonJoinedTime);
                DemonJoinedFlag = true;
            }
        }
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable SentHash){
        for (int i = 0; i < PlayerInfoList.Count; ++i){
            
            string value = (string)SentHash[PlayerInfoList[i].PViewId.ToString()];
            if (value == null){ //これないとエラー出るから気を付けて
                Debug.Log("nullなんよそれ");
            }
            else if (value == CreatePlayerValue(i)){
                Debug.Log("同じやつ来てる");
            }
            else if (value != CreatePlayerValue(i)){
                UpdatePlayerInfoList(i, value);
                Debug.Log(CreatePlayerValue(i) + "更新した");
            }
            Debug.Log("何回");
        }
        Debug.Log("nannkai");
        string DTime = (string)SentHash["DemonJoinedTime"];
        if (DTime == null){}
        else {
            DemonJoinedTime = int.Parse(DTime);
            DemonJoinedFlag = true;
        }
        Debug.Log(SentHash);
    }
    void Update()
    {
        switch (Game_Statue){
            case Status.before:{
                if (Input.GetKeyDown(KeyCode.F)){   //無理やりスタート
                    GameStartFlag = true;
                }
                if (Input.GetKeyDown(KeyCode.C)){   //プレイヤーのリストを作るタイミングで押す
                    CreatePlayerList();
                    Debug.Log(move.PlayerViewIdsList.Count);
                }                
                break;
            }
            case Status.play:{
                break;
            }
            case Status.after:{
                break;
            }
        }

        CheckMyViewId();

        if (Input.GetKeyDown(KeyCode.G)){   //これもテスト
            Debug.Log(GetRoomProperty("DemonJoinedTime"));
        }
        if (Input.GetKeyDown(KeyCode.L)){
            win_or_loss_decision();
        }
        if (Input.GetKeyDown(KeyCode.P)){
            for (int i = 0; i < 20; i++){
                Debug.Log(TreasureChest[i]);
            }
        }
        if (JoinRoomFlag == true){
            CurrentTime = PhotonNetwork.ServerTimestamp;
        }
        if (DemonJoinedFlag == true){
            if (CurrentTime - DemonJoinedTime > 240000){
                win_or_loss_decision();
            }
        }
        GameStart();
        if (GameStartFlag == true){
            if (StartCount == false){
                CreatePlayerList();
                StartCount = true;
            }
        }
        else {
            Debug.Log("待機中");
        }
        GameEnd();
    }

    void game_start_up(){
        int player_int_num = Scene_mane_Script.SelectPD;
        if (player_int_num == 0){
            //逃げる側を動かす
            main_play_samon();            
        }
        else {
            //追いかける側を動かす
            Demon_samon();            
        }
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

    void win_or_loss_decision(){
        bool p_loss = true;
        foreach(var a in PlayerInfoList){
            if(!a.PCatchFlag)p_loss = false;
        }
        if(p_loss)decision = 0;
        else decision = 1;
        move.PlayerViewIdsList.Clear();
        GameObject.FindWithTag("scene_mane").GetComponent<Scene_mane_Script>().scene_num = 2;
        GameObject.FindWithTag("scene_mane").GetComponent<Scene_mane_Script>().scene_chanz = true;
        
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
        if (DemonFlag == false){
            for (int i = 1; i < move.PlayerViewIdsList.Count; ++i){
            Player2 = new PlayerInfo(move.PlayerViewIdsList[i], 0, false);
            PlayerInfoList.Add(Player2);
            }
        }
        else {
            for (int i = 0; i < move.PlayerViewIdsList.Count; ++i){
            Player2 = new PlayerInfo(move.PlayerViewIdsList[i], 0, false);
            PlayerInfoList.Add(Player2);
            } 
        }
        CreatePlayerListFlag = true;
    }

    /// <summary>
    /// ただプレイヤー情報リストの長さを取りたい。
    /// </summary>
    /// <returns></returns>
    public int GetPlayerInfoListCount(){
        int Count = 0;
        Count = PlayerInfoList.Count;
        return Count;
    }

    /// <summary>
    /// PlayerInfoListのインデックスからViewIdを取る。
    /// </summary>
    /// <param name="PlayerInfoListIndex">取りたいプレイヤーのインデックス</param>
    /// <returns></returns>
    public int GetPlayerViewIdFromListByIndex(int PlayerInfoListIndex){
        int ViewId = 0;
        ViewId = PlayerInfoList[PlayerInfoListIndex].PViewId;
        return ViewId;
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
        int Index = GetPlayerInfoIndexFromViewId(PlayerViewId);
        if (GetElement == "CatchFlag"){
            Content = PlayerInfoList[Index].PCatchFlag.ToString();
        }
        else if(GetElement == "HidePlace"){
            Content = PlayerInfoList[Index].PHidePlace.ToString();
        }
        else {
            Debug.Log("naiyo" + PlayerViewId);
        }
        return Content;
    }

    /// <summary>
    /// プレイヤー情報をインデックス指定で持ってくる
    /// </summary>
    /// <param name="PlayerInfoListIndex">持ってきたいプレイヤーのインデックス</param>
    /// <param name="GetElement">持ってきたい要素、HidePlace,CatchFlag</param>
    /// <returns></returns>
    public string GetPlayerInfoFromIndex(int PlayerInfoListIndex, string GetElement){
        string Content = "";
        if (GetElement == "CatchFlag"){
            Content = PlayerInfoList[PlayerInfoListIndex].PCatchFlag.ToString();
        }
        else if(GetElement == "HidePlace"){
            Content = PlayerInfoList[PlayerInfoListIndex].PHidePlace.ToString();
        }
        else {
            Debug.Log("naiyo" + PlayerInfoListIndex + GetElement);
        }
        return Content;
    }

    /// <summary>
    /// プレイヤーのViewIdをから、PlayerInfoListのIndexを返す。
    /// </summary>
    /// <param name="PlayerViewId"></param>
    /// <returns></returns>
    public int GetPlayerInfoIndexFromViewId(string PlayerViewId){
        int Index = 0;
        for (int i = 0; i < PlayerInfoList.Count; ++i){
            if (PlayerViewId == PlayerInfoList[i].PViewId.ToString()){
                Index = i;
            }
        }
        return Index;
    }

    /// <summary>
    /// プレイヤーのViewIdを指定して、情報の更新と送信をする
    /// </summary>
    /// <param name="PlayerViewId">プレイヤーのViewId</param>
    /// <param name="UpdateElement">更新したい要素、HidePlace,CatchFlag</param>
    /// <param name="UpdateContent">更新内容</param>
    public void UpdatePlayerInfoAndHash(string PlayerViewId, string UpdateElement, string UpdateContent){
        int Index = GetPlayerInfoIndexFromViewId(PlayerViewId);
        if (UpdateElement == "HidePlace"){
            ChangePlayerList(Index, "HidePlace", UpdateContent);
        }
        else if (UpdateElement == "CatchFlag"){
            ChangePlayerList(Index, "CatchFlag", UpdateContent);
        }
        else {
            Debug.Log("更新できてない" + PlayerViewId + UpdateElement);
        }
        SendPlayerInfo(Index);
    }

    /// <summary>
    /// PlayerInfoListのIndex指定して、情報の更新だけする。
    /// </summary>
    /// <param name="PlayerInfoListIndex">プレイヤーのインデックス</param>
    /// <param name="UpdateElement">更新したい要素、HidePlace,CatchFlag</param>
    /// <param name="UpdateContent">更新内容</param>
    public void UpdatePlayerInfoListByIndex(int PlayerInfoListIndex, string UpdateElement, string UpdateContent){
        if (UpdateElement == "HidePlace"){
            ChangePlayerList(PlayerInfoListIndex, "HidePlace", UpdateContent);
        }
        else if (UpdateElement == "CatchFlag"){
            ChangePlayerList(PlayerInfoListIndex, "CatchFlag", UpdateContent);
        }
        else {
            Debug.Log("更新できてない" + PlayerInfoListIndex + UpdateElement);
        }
    }

    /// <summary>
    /// ゲームスタート
    /// </summary>
    void GameStart(){
        if (DemonJoinedFlag == true){
            if (GameStartFlag == false){
                if (CurrentTime - DemonJoinedTime > 60000){
                    GameStartFlag = true;
                }
                if (GameStartFlag == true){
                    Debug.Log("ゲームスタート");
                }
            }
            if (DemonCatchStartFlag == false){
                if (CurrentTime - DemonJoinedTime > 62000){
                    DemonCatchStartFlag = true;
                }                
            }
        }
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    void GameEnd(){
        if (GameEndFlag == false && CreatePlayerListFlag){
            for (int i = 0; i < PlayerInfoList.Count; ++i){
                if (PlayerInfoList[i].PCatchFlag == true){
                    int PlayerCount = 0;
                    PlayerCount += 1;
                    if (PlayerCount == PlayerInfoList.Count){
                        Debug.Log("ゲーム終了");
                        
                        GameEndFlag = true;                        
                    }
                }
            }
        }

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
        if (roomHash[name] == null){
            roomHash[name] = value;
            // ルームにハッシュを送信する        
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);  
        }
        else if ((int)roomHash[name] == value){
            Debug.Log("同じの送ってる" + value);
        }
        else {
            roomHash[name] = value;
            // ルームにハッシュを送信する        
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);  
        }
    }
    public void Propeties_Hash_bool(string name,bool value)
    {
        roomHash[name] = value;
        // ルームにハッシュを送信する        
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);        
    }

    /// <summary>
    /// カスタムプロパティから指定したキーのバリューを持ってくる。なにもないときは、"Null"がかえる
    /// </summary>
    /// <param name="Key">持ってきたいバリューのキー</param>
    /// <returns></returns>
    public object GetRoomProperty(string Key){
        object ValueObj = PhotonNetwork.CurrentRoom.CustomProperties[Key];
        if (ValueObj == null){
            Debug.Log("ぬるなんよそれ");
            return ValueObj;
        }
        else {
            return ValueObj;
        }
    }

    void CreateTreasureChestList(){
        for (int i = 0; i < 20; i++)TreasureChest.Add("000");
        CreateTreasureChestListflag = true;
    }

    public void ChangeTreasureChestList(int index, string ItemInfo){
        TreasureChest[index] = ItemInfo;
    }
}

