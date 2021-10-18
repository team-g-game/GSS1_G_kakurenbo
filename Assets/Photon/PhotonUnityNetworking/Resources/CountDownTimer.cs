
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
	timer down_timer;
	
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
		if (Game_cont.Game_Status == Game_cont.Status.before && GameObject.Find("Game_master").GetComponent<Game_cont>().DemonFlag){
			if(down_timer != timer.start && start_one && totalTime ==0){
				//　トータル制限時間
				totalTime = 1 * 60 + 0;
				down_timer = timer.start;
				start_one = false;
			}
			
			string retimer = count_Down();
			if(retimer == "stop"){
				retimer = "00:00";
				GameObject.Find("Game_master").GetComponent<Game_cont>().GameStart();
			}
			timerText.text = retimer;
		}
		if (Game_cont.Game_Status == Game_cont.Status.play){
			if(down_timer != timer.start&& start_one == false && totalTime ==0){
				//　トータル制限時間
				totalTime = minute * 60 + seconds;
				Debug.Log("ここ定義:" + totalTime);
				down_timer = timer.start;	
				start_one = true;			
			}

			string retimer = count_Down();
			if(retimer == "stop"){
				retimer = "00:00";
				GameObject.Find("Game_master").GetComponent<Game_cont>().win_or_loss_decision();
			}
			timerText.text = retimer;
		}

	}
	string count_Down(){
		string time_text = "";
		switch (down_timer){
			case timer.start:
			{
				if(totalTime <=0)down_timer = timer.stop;
				else
				{
					totalTime -= Time.deltaTime;
					Debug.Log(totalTime);
					time_text = ((int)totalTime/60).ToString("00") + ":" + ((int)totalTime%60).ToString("00");
				}

				return time_text;
			}
			case timer.stop:
			{
				Debug.Log("ストップはいりました");
				if(totalTime <= 0)
				{
					time_text = "stop";
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
