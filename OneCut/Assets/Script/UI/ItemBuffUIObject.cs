using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuffUIObject : UIObject
{
    public Image img;

    public void Set(ItemData itemData)
    {            
        img.sprite = itemData.icon; 
    }
}
