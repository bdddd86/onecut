using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopObject : MonoBehaviour 
{
    public Button btnBuy;
    public Image imgItem;
    public Text lblName;
    public Text lblPrice;

    ShopItem shopItem;

    private void Start()
    {
        btnBuy.onClick.AddListener(Buy);
    }

    public void Set(ShopItem item)
    {
        this.shopItem = item;
        imgItem.sprite = shopItem.icon;
        lblName.text = UtillFunc.Instance.GetLocalizedText(shopItem.name);
        lblPrice.text = UtillFunc.Instance.GetPriceText(shopItem.price);
    }

    void Buy()
    {
        //item buy
    }
}    

