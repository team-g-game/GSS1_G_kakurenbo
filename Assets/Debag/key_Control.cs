using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key_Control : MonoBehaviour
{
    GameObject menu;
    GameObject Esc_menu;
    // Start is called before the first frame update
    void Start()
    {        
        Cursor_visi(false);
        menu = transform.Find("Menu").gameObject;
        Esc_menu = menu.transform.Find("Esc_menu").gameObject;
        Esc_menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Esc_menu.activeInHierarchy){
                Esc_menu.SetActive(false);
                Cursor_visi(false);
            }else {
                Esc_menu.SetActive(true);
                Cursor_visi(true);
            }
        }
    }
    void Cursor_visi(bool valu){
        if (valu){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
