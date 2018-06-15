using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIObject : MonoBehaviour {
    public Image img;

    public void Set(string item)
    {
        ItemData itemData = ItemManager.instance.Find(item);
        if(itemData != null)
        {
            img.sprite = itemData.icon; 
        }
    }
}
