using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Item item;

    [SerializeField]
    private Image itemImage;

    public Item MyItem { get => item; private set => item = value; }

    public void SetItem(Item item)
    {
        MyItem = item;

        if(item!=null)
        {
            itemImage.color = new Color(1,1,1,1);
             itemImage.sprite = item.MyItemImage;
        }
        else
        {
            itemImage.color = new Color(0,0,0,0);
        }
    }
}