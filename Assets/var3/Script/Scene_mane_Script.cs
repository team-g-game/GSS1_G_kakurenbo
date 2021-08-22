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
    private string maci_scene_name = "Prototype_var1";
    private string rizarut_scene_name = "rizarut";
    
    private string game_stage = "Pro_ver_2";
    public int scene_num = 0;
    public bool scene_chanz = false;

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
            //if (scene_num == 1) SceneManager.LoadScene(maci_scene_name);
            if (scene_num == 2) SceneManager.LoadScene(rizarut_scene_name);
            if (scene_num == 1) SceneManager.LoadScene(game_stage);
        }
    }
    public void title_clikc(){
        scene_num = 1;
        scene_chanz = true;
    }
}
