using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Realtime;
using Photon.Pun;


public class Scene_mane_Script : MonoBehaviour
{
    private string title_scene_name = "title";
    private string new_rizarut_scene_naem = "result";
    private string game_stage = "Pro_ver_2";
    public int scene_num = 0;
    public bool scene_chanz = false;
    public static int SelectPD;

    public AudioClip ButtonSE;

    // Start is called before the first frame update
    void Awake(){
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (scene_chanz){
            scene_chanz = false;
            if (scene_num == 0) SceneManager.LoadScene(title_scene_name);
            if (scene_num == 2) {
                SceneManager.LoadScene(new_rizarut_scene_naem);
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
                
            }
            if (scene_num == 1) SceneManager.LoadScene(game_stage);
        }
    }
    public void title_clikc(){
        AudioSource.PlayClipAtPoint( ButtonSE, transform.position);  // 効果音を鳴らす
        SelectPD = Title_button_choise.SelectPAndD;
        scene_num = 1;
        scene_chanz = true;
    }

}
