
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;

public class CountDownTimer : MonoBehaviour {

	//　制限時間（分）
	[SerializeField]
	private int minute;
	//　制限時間（秒）
	[SerializeField]
	private int seconds;
	private Text timerText;
	static timer down_timer;
	
	enum  timer
	{
		start = 0,
		stop = 1
	}
	static float totalTime;
	public static bool start_ok;
	public static bool end_ok;
	float back_time;
	bool start_one = true;
	(int,int) _minsec;
	void Start () {

		timerText = GetComponentInChildren<Text>();
		down_timer = timer.stop;
		_minsec = (minute,seconds);
		back_time = (float)PhotonNetwork.Time;
		start_ok = false;
		end_ok = false;
		
	}

	void Update () {
		if(GameObject.Find("Game_master").GetComponent<Game_cont>().DemonFlag){
			timer_chack();
			timerText.text = count_Down();
		}else{
			if(totalTime <= 0)timerText.text = "00:00";
			else timerText.text = ((int)totalTime/60).ToString("00") + ":" + ((int)totalTime%60).ToString("00");
		}
		Debug.Log(totalTime +":"+ Game_cont.Game_Status);
	}
	int timer_chack(){
		switch (Game_cont.Game_Status){
			case Game_cont.Status.before:{
				if(down_timer == timer.stop){
					if(start_one){
						Debug.Log("待機カウントスタート");
						//　トータル制限時間
						totalTime = 1 * 60 + 0;
						down_timer = timer.start;
						start_one = false;					
					}
				}
				if(totalTime <= 0&&start_one == false)start_ok = true;
				break;
			}
			
			case Game_cont.Status.play:{
				if(down_timer == timer.stop){
					if(start_one == false){
						//　トータル制限時間
						totalTime = _minsec.Item1 * 60 + _minsec.Item2;
						Debug.Log("ここ定義:" + totalTime);
						down_timer = timer.start;	
						start_one = true;	
					}
				}
				if(totalTime <= 0 && start_one)end_ok = true;
				break;
			}
			case Game_cont.Status.after:{
				break;
			}
		}
		return 1;
	}
	string count_Down(){
		string time_text = "";
		switch (down_timer){
			case timer.start:
			{
				if(totalTime <=0)
				{
					down_timer = timer.stop;
					time_text = "00:00";
				}
				else
				{
					totalTime -= (float)PhotonNetwork.Time - back_time;
					//Debug.Log($"残り時間{totalTime}秒");
					time_text = ((int)totalTime/60).ToString("00") + ":" + ((int)totalTime%60).ToString("00");
					back_time = (float)PhotonNetwork.Time;
				}
				return time_text;
			}
			case timer.stop:
			{
				Debug.Log("ストップはいりました");
				if(totalTime <= 0)
				{
					time_text = "00:00";
					totalTime = 0;
				}else time_text = ((int)totalTime/60).ToString("00") + ":" + ((int)totalTime%60).ToString("00");
				return time_text;
			}
			default:
				time_text = "00:00";
				return time_text;
		}
	}
}
