using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

[System.Serializable]
public struct ShopItem
{
    public string name;
    public Sprite icon; 
    public int price;
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
}
