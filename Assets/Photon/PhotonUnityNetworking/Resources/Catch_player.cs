using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch_player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider hako)
    {
        if (hako.CompareTag("Player"))
        {
        }
    }
}
