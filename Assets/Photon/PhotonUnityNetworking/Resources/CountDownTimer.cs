
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;

//clock_contから取ってくる;
public class CountDownTimer : MonoBehaviour {
	private Text timerText;
	void Start(){
		timerText = GetComponentInChildren<Text>();
	}
	void Update(){
		//Debug.Log(clock_cont.num);
		timerText.text = ((int)clock_cont.num/60).ToString("00") + ":" + ((int)clock_cont.num%60).ToString("00");
	}
}
