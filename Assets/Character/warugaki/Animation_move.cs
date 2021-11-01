using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_move : MonoBehaviour {

	// Animator コンポーネント
	private Animator animator;
	private CharacterController characterController;
	// 設定したフラグの名前
	//private const string key_isRun = "Walk";
    private float sp = 1.0f;

	// 初期化メソッド
	void Start () {
		// 自分に設定されているAnimatorコンポーネントを習得する
		animator = GetComponent<Animator>();
		characterController = GetComponent<CharacterController>();

	}
	
	void Update () {

		if (Input.GetKey("up"))
        {
			animator.SetFloat("Speed", sp);
			transform.position += transform.forward * sp * Time.deltaTime;

		}else{
			animator.SetFloat("Speed", 0f);
		}


	}
}
