
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {

	//　制限時間（分）
	[SerializeField]
	private int minute;
	//　制限時間（秒）
	[SerializeField]
	private float seconds;
	private Text timerText;
	static timer down_timer = timer.stop;
	
	enum  timer
	{
		start = 0,
		stop = 1
	}
	static float totalTime = 0;
	bool start_one = true;
	void Start () {
		Debug.Log("始まった");
		timerText = GetComponentInChildren<Text>();
	}

	void Update () {
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
						totalTime = minute * 60 + seconds;
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
		string retimer = count_Down();
		timerText.text = retimer;

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
