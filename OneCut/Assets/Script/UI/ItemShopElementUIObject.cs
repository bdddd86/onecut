using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemShopElementUIObject : UIObject 
{
    public RectTransform pnlDisabled;
    public Button btnBuy;
    public Image imgItem;
    public Text lblLevelText;
    public Text lblName;
    public Text lblPrice;
    public Text lblDescription;

    [HideInInspector]
    public ItemData itemData;

    private void Start()
    {
        btnBuy.onClick.AddListener(Buy);
    }

    public void Set(ItemData itemData)
    {
        pnlDisabled.gameObject.SetActive(itemData.levelLimit > GameManager.instance.Level);
        lblLevelText.text = UtillFunc.Instance.GetLocalizedText("레벨 {0}에 구매 가능", itemData.levelLimit); 

        this.itemData = itemData;
        imgItem.sprite = itemData.icon;
        lblName.text = UtillFunc.Instance.GetLocalizedText(itemData.name);
        lblDescription.text = itemData.description; 

        lblPrice.text = UtillFunc.Instance.GetPriceText(itemData.price);
        bool enoughGold = GameManager.instance.hasGold >= itemData.price;
        lblPrice.color = UtillFunc.Instance.ConvertShortageValueColor(enoughGold);

        btnBuy.interactable = enoughGold;

    }

    void Buy()
    {
        GameManager.instance.BuyItem(itemData);  
    }
}    

