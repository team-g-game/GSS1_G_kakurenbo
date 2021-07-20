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
    private string room_choise = "room choise";
    private string room_scene_name = "kyara_taik";
    private string maci_scene_name = "Prototype_var1";
    private string rizarut_scene_name = "rizarut";
    public int scene_num = 0;
    public bool scene_chanz = false;
    public float sleep_time = 5f ;//秒

    // Start is called before the first frame update
    void Awake(){
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Connect("v1.0");
    }
    private void Connect(string varsion){
        if(PhotonNetwork.IsConnected == false){
            PhotonNetwork.GameVersion = varsion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        sleep_time -= Time.deltaTime;
        if (scene_chanz){
            scene_chanz = false;
            if (scene_num == 0) SceneManager.LoadScene(title_scene_name);
            if (scene_num == 1) SceneManager.LoadScene(room_choise);
            if (scene_num == 2) SceneManager.LoadScene(room_scene_name);
            if (scene_num == 3) SceneManager.LoadScene(maci_scene_name);
            if (scene_num == 4) SceneManager.LoadScene(rizarut_scene_name);
        }
        
        if (Input.GetMouseButton(1)&&sleep_time <=0){
            sleep_time = 5f;
            if (scene_num == 3) scene_num = 1;
            else scene_num +=1;
            scene_chanz = true;

        }
    }
    public void title_clikc(){
        
        scene_num = 1;
        scene_chanz=true;
        

    }
}
