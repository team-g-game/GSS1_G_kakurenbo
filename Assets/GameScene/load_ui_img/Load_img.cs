using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class Load_img : MonoBehaviour
{
    public List<Sprite> imgs;
    public int load_img_num;
    public GameObject img_show;
    private float _timer = 0;
    
    //ミリ秒
    public int load_img_interval;
    // Start is called before the first frame update
    void Start()
    {
        img_show.GetComponent<Image>().sprite = imgs[load_img_num];
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime * 1000;
        if(load_img_interval <= _timer){
            _timer = 0;
            load_img_num ++;
            if(load_img_num >= imgs.Count) load_img_num = 0;

            img_show.GetComponent<Image>().sprite = imgs[load_img_num];
        }
    }
}
