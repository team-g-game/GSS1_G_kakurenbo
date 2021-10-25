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
        public string ItemInfo;

        public PlayerInfo(int PlayerViewId, int HidePlace, bool CatchFlag, string ItemInfomation){
            PViewId = PlayerViewId;
            PHidePlace = HidePlace;
            PCatchFlag = CatchFlag;
            ItemInfo = ItemInfomation;
            
        }
    }
    private bool CheckCreatId = false;  //一回だけ自分の情報を作って送るためにある
    PlayerInfo Myself;  //入った本人の情報
    PlayerInfo Player2; //ほかのプレイヤーの情報を作るために作ったけど要らない気がしてきた
    List<PlayerInfo> PlayerInfoList = new List<PlayerInfo>();   //プレイヤーの全情報が入ってる
    List<string> TreasureChest = new List<string>();
    public  bool JoinRoomFlag = false;    //ルームに参加したタイミングを判定
    public bool DemonFlag = false;       //鬼側のフラグ
    public bool CreatePlayerListFlag = false;    //プレイヤーリストを生成したタイミングを判定
    public int DemonJoinedTime = 0;     //鬼がルームに入ってきた時間
    public int CurrentTime;             //ルームの現在時刻
    public static bool DemonJoinedFlag; //鬼が入ってきたかどうか判定
    
    /// <value>ゲームの状態で見るように書き換える</value>
    public static bool decision;// 0なら鬼の勝ち
    public bool CatchPlayerFlag = false;
    public bool CreateTreasureChestListflag = false;

    /// <summary>ゲームの状態</summary>
    public static Status Game_Status;
    /// <summary>状態</summary>
    public enum Status 
    {
        /// <value>ゲーム開始前</value>
        before = 0,
        /// <value>ゲーム中</value>
        play = 1,
        /// <value>ゲーム後</value>
        after = 2
    }
    [SerializeField]
    int start_time;
    [SerializeField]
    int game_time_min;
    [SerializeField]
    int game_time_sec;

    void Start()
    {
        DemonJoinedFlag = false;
        decision = false;
        Game_Status = Status.before;
        
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
            CreateTreasureChestListHashtable();
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
        for (int i = 0; i < TreasureChest.Count; i++){
            string value = (string)SentHash[i.ToString()];
            if (value == null){ //これないとエラー出るから気を付けて
                Debug.Log("nullなんよそれ");
            }
            else if (value == TreasureChest[i]){
                Debug.Log("同じやつ来てる");
            }
            else if (value != TreasureChest[i]){
                ChangeTreasureChestList(i, value);
                Debug.Log(TreasureChest[i] + "更新した");
            }
            Debug.Log("何回か");
        }
        Debug.Log("nannkai");
        string DTime = (string)SentHash["DemonJoinedTime"];
        if (DTime == null){}
        else {
            DemonJoinedTime = int.Parse(DTime);
            DemonJoinedFlag = true;
            GameObject.FindWithTag("clock").GetComponent<clock_cont>().timer_start(PhotonNetwork.ServerTimestamp.ToString() + "," + (PhotonNetwork.ServerTimestamp + start_time * 1000).ToString());
        }
        Debug.Log(SentHash);
    }
    void Update()
    {
        switch (Game_Status){
            case Status.before:{
                CheckMyViewId();
                if (Input.GetKeyDown(KeyCode.F)){   //無理やりスタート
                    GameStart();
                }
                if (Input.GetKeyDown(KeyCode.C)){   //プレイヤーのリストを作るタイミングで押す
                    CreatePlayerList();
                    Debug.Log(move.PlayerViewIdsList.Count);
                }
                if(clock_cont.num < 0){
                    GameStart();
                }
                //if(CountDownTimer.start_ok)GameStart();
                Debug.Log("待機中");
                break;
            }

            case Status.play:{
                //プレイヤーのアイテム確認？
                if (Input.GetKeyDown(KeyCode.P)){
                    for (int i = 0; i < 20; i++){
                        Debug.Log(TreasureChest[i]);
                    }
                }
                if (Input.GetKeyDown(KeyCode.L))win_or_loss_decision();
                if (Input.GetKeyDown(KeyCode.G))Debug.Log(GetRoomProperty("DemonJoinedTime"));
                if(clock_cont.num < 0){
                    win_or_loss_decision();
                }
                GameEnd();
                break;
            }
            case Status.after:{
                break;
            }
        }
        if (JoinRoomFlag == true){
            CurrentTime = PhotonNetwork.ServerTimestamp;
        }
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

    public void win_or_loss_decision(){
        bool p_loss = true;
        foreach(var a in PlayerInfoList){
            if(!a.PCatchFlag)p_loss = false;
        }
        if(p_loss)decision = false;
        else decision = true;

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
                Myself = new PlayerInfo(move.MyPlayerViewId, 0, false, "000");
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
        else if(ChangeElement == "ItemInfo"){
            example.ItemInfo = ChangeContent;
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
        ChangePlayerList(PlayerNum, "ItemInfo", part[2]);
    }

    /// <summary>
    /// 全プレイヤーの情報リストを作成
    /// </summary>
    void CreatePlayerList(){
        if (DemonFlag == false){
            for (int i = 1; i < move.PlayerViewIdsList.Count; ++i){
            Player2 = new PlayerInfo(move.PlayerViewIdsList[i], 0, false, "000");
            PlayerInfoList.Add(Player2);
            }
        }
        else {
            for (int i = 0; i < move.PlayerViewIdsList.Count; ++i){
            Player2 = new PlayerInfo(move.PlayerViewIdsList[i], 0, false, "000");
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
        PlayerValue += PlayerInfoList[PlayerNum].PCatchFlag + ",";
        PlayerValue += PlayerInfoList[PlayerNum].ItemInfo;
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
        else if (GetElement == "ItemInfo"){
            Content = PlayerInfoList[Index].ItemInfo;
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
        else if (GetElement == "ItemInfo"){
            Content = PlayerInfoList[PlayerInfoListIndex].ItemInfo;
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
        else if (UpdateElement == "ItemInfo"){
            ChangePlayerList(Index, "ItemInfo", UpdateContent);
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
        else if (UpdateElement == "ItemInfo"){
            ChangePlayerList(PlayerInfoListIndex, "ItemInfo", UpdateContent);
        }
        else {
            Debug.Log("更新できてない" + PlayerInfoListIndex + UpdateElement);
        }
    }

    /// <summary>
    /// ゲームスタート
    /// </summary>
    public void GameStart(){
        if (DemonJoinedFlag == true){
            if (Game_Status == Status.before /*&& CountDownTimer.start_ok*/){
                
                int start_taik = ( game_time_min * 60 + game_time_sec )* 1000;
                GameObject.FindWithTag("clock").GetComponent<clock_cont>().timer_start(PhotonNetwork.ServerTimestamp.ToString() + "," + (PhotonNetwork.ServerTimestamp + start_taik).ToString());
        
                Game_Status = Status.play;
                CreatePlayerList();
                Debug.Log("ゲームスタート");
                
            }
        }
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    void GameEnd(){
        if (Game_Status != Status.after && CreatePlayerListFlag){
            for (int i = 0; i < PlayerInfoList.Count; ++i){
                if (PlayerInfoList[i].PCatchFlag == true){
                    int PlayerCount = 0;
                    PlayerCount += 1;
                    if (PlayerCount == PlayerInfoList.Count /*|| CountDownTimer.end_ok*/){
                        win_or_loss_decision();
                        Debug.Log("ゲーム終了");
                        
                        Game_Status = Status.after;                       
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

    /// <summary>
    /// TreasureChestListの内容を変更。
    /// </summary>
    /// <param name="index">変更したいTreasureChestの番号</param>
    /// <param name="ItemInfo">変更したい内容</param>
    public void ChangeTreasureChestList(int index, string ItemInfo){
        TreasureChest[index] = ItemInfo;
    }

    /// <summary>
    /// TresureChestListの内容変更をハッシュに送信。
    /// </summary>
    /// <param name="index">送信したいTreasureChestの番号</param>
    /// <param name="ItemInfo">送信したい内容</param>
    public void SendTreasureChestList(int index, string ItemInfo){
        Propeties_Hash_string(index.ToString(), ItemInfo);
    }

    /// <summary>
    /// TreasureChestListをハッシュにすべて送る。
    /// </summary>
    void CreateTreasureChestListHashtable(){
        for (int i = 0; i < 20; i++){
            string TreasureKey = i.ToString();
            Propeties_Hash_string(TreasureKey, TreasureChest[i]);
        }
    }
}

