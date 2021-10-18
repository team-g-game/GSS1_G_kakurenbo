
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	static bool start_one;
	static (int,int) _minsec;
	void Start () {

		timerText = GetComponentInChildren<Text>();
		down_timer = timer.stop;
		_minsec = (minute,seconds);
		totalTime = 0;
		start_one = true;
	}

	void Update () {
		timer_chack();
		timerText.text = count_Down();
	}
	static int timer_chack(){
		switch (Game_cont.Game_Status){
			case Game_cont.Status.before:{
				if(GameObject.Find("Game_master").GetComponent<Game_cont>().DemonFlag){
					if(down_timer == timer.stop){
						if(start_one){
							Debug.Log("aaa");
							//　トータル制限時間
							totalTime = 1 * 60 + 0;
							down_timer = timer.start;
							start_one = false;					
						}else{
							Game_cont.Game_Status =Game_cont.Status.play;
						}
					}
				}
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
					}else{
						if(totalTime <= 0)GameObject.Find("Game_master").GetComponent<Game_cont>().win_or_loss_decision();
					}
				}
				break;
			}
			case Game_cont.Status.after:{
				break;
			}
		}
		return 1;
	}
	static string count_Down(){
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
					totalTime -= Time.deltaTime;
					Debug.Log($"残り時間{totalTime}秒");
					time_text = ((int)totalTime/60).ToString("00") + ":" + ((int)totalTime%60).ToString("00");
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
