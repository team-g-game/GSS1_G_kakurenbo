using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_contlol : MonoBehaviour
{
    public bool key_contlol = false;
    [SerializeField] Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(key_contlol){
            var pos = key_move(transform.position);
            transform.position = pos;
        } 
        
    }
    public Vector3 key_move(Vector3 pos)
    {
        if(Input.GetKey(KeyCode.W)) pos += transform.forward * Time.deltaTime;
        if(Input.GetKey(KeyCode.S)) pos -= transform.forward * Time.deltaTime;
        if(Input.GetKey(KeyCode.A)) pos -= transform.right * Time.deltaTime;
        if(Input.GetKey(KeyCode.D)) pos += transform.right * Time.deltaTime;
        if(Input.GetKey(KeyCode.Space)) pos.y = 10;
        return pos;
    }
}
