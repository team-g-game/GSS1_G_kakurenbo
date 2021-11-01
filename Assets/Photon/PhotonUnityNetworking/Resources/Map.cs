using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 　

public class Map : MonoBehaviour
{
    public GameObject button;


    public void OnClickStartButton()
    {

        button.SetActive(false);
        Debug.Log("押した");

    }
}