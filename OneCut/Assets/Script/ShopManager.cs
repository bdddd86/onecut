﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopItem
{
    public string key;
    public int price;
    private ItemData _itemData = null;
    public ItemData itemData
    {
        get
        {
            if(_itemData == null)
            {
                _itemData = ItemManager.instance.Find(key);
            }
            return _itemData; 
        }
    }
}    

public class ShopManager : MonoBehaviour {

    public Button btnClose;
    public List<ShopItem> shopItemDatas;

    public ScrollRect shopScroll;
    public GameObject shopObjPrefab;

    private List<ShopObject> shopObjectList = new List<ShopObject>(); 

	void Start () {
        btnClose.onClick.AddListener(Close);

        shopObjectList.Clear();
        for (int i = 0; i < shopItemDatas.Count; ++i)
        {
            ShopObject obj = Instantiate(shopObjPrefab, Vector3.zero, Quaternion.identity).GetComponent<ShopObject>();
            obj.transform.parent = shopScroll.content;
            obj.Set(shopItemDatas[i]);
            shopObjectList.Add(obj);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Close()
    {
        GameManager.instance.CloseShop();
    }

    public void BuyItem(string key)
    {
        int findIndex = shopObjectList.FindIndex(e => e.shopItem.key == key);
        if (findIndex >= 0)
        {
            DestroyObject(shopObjectList[findIndex].gameObject);
            shopObjectList.RemoveAt(findIndex);
        }
    }
}