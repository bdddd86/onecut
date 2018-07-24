using System.Collections;
using System.Collections.Generic;
using System.Text; 
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int id; 
    public string name;
    public int price;
   
    public int levelLimit;
    public int abilityDataId;
    public AbilityTypeData abilityTypeData
    {
        get
        {
            if(_abilityTypeData == null)
            {
                _abilityTypeData = DataManager.instance.abilityTypeData.Find(e => e.id == abilityDataId); 
            }
            return _abilityTypeData; 
        }
    }

    AbilityTypeData _abilityTypeData; 
    public int param;

    public string description
    {
        get
        {
            if (_description == string.Empty)
            {
                if (abilityTypeData.abilityType == AbilityType.Specific)
                {
                    _description = "특수능력";
                }
                else
                {
                    _description = string.Format("{0}+{1}{2}", abilityTypeData.abilityType.ToString()
                                                 , param
                                                 , abilityTypeData.calcurateType == CalcurateType.Product ? "%" : "");
                }
            }
            return _description; 
        }
    }
    string _description = string.Empty;

    public Sprite icon;

    // 이전 데이터
    public string key; 
}

public class ItemManager : MonoSingleton<ItemManager> 
{
    public List<ItemData> itemDatas; 
    public ItemData Find(string key)
    {
        return itemDatas.Find(e => e.key == key);
    }
}
