using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string key;
    public string name;
    public Sprite icon;
}

public class ItemManager : MonoSingleton<ItemManager> 
{
    public List<ItemData> itemDatas; 
    public ItemData Find(string key)
    {
        return itemDatas.Find(e => e.key == key);
    }
}
