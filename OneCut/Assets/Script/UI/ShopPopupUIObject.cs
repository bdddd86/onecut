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

public class ShopPopupUIObject : UIObject 
{
    public Button btnClose;
    public ScrollRect shopScroll;
    public GameObject shopObjPrefab;

    private List<ItemShopElementUIObject> shopObjectList = new List<ItemShopElementUIObject>(); 

	void Start () {
        btnClose.onClick.AddListener(Close);

        shopObjectList.Clear();
        for (int i = 0; i < ItemManager.instance.itemDatas.Count; ++i)
        {
            ItemShopElementUIObject obj = Instantiate(shopObjPrefab, Vector3.zero, Quaternion.identity).GetComponent<ItemShopElementUIObject>();
            obj.transform.parent = shopScroll.content;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
            obj.Set(ItemManager.instance.itemDatas[i]);
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

    public void BuyItem(ItemData itemData)
    {
        int findIndex = shopObjectList.FindIndex(e => e.itemData.id ==  itemData.id);
        if (findIndex >= 0)
        {
            DestroyObject(shopObjectList[findIndex].gameObject);
            shopObjectList.RemoveAt(findIndex);
        }
    }
}
