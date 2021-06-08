using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play_cam_cont : MonoBehaviour
{
    public GameObject p ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionStay(Collision a){
        transform.LookAt(p.transform);
    }
    void OnCollisionExit(Collision a){
        var pos = transform.localPosition;
        pos = p.transform.position - transform.forward * 5;
        transform.localPosition = pos;
        transform.LookAt(p.transform);
    }
}
