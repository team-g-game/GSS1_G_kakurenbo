using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_move : MonoBehaviour {

	// Animator コンポーネント
	private Animator animator;

	// 設定したフラグの名前
	//private const string key_isRun = "Walk";
    private float speed = 1.0f;

	// 初期化メソッド
	void Start () {
		// 自分に設定されているAnimatorコンポーネントを習得する
		this.animator = GetComponent<Animator>();
	}
	
	void Update () {


		if (Input.GetKey(KeyCode.UpArrow))
        {
			this.animator.SetBool("Walk", true);


		}

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
		
		else {
			this.animator.SetBool("Walk", false);
        }

	}
}
