using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> {

    public ShopManager shopManager;
    public Transform spawnPoint;

    //public GameObject objChacater;
	public Character character;
    public GameObject blackScreen;
    public Text gameInfoText;

    public List<ItemUIObject> inventoryDisplay; 

	[Header("[몬스터 데미지]")]
	public List<DamageText> listDamageText;
	private int lastDamageText = 0;

    List<ItemData> inventory;

	//[HideInInspector] public Character character;

	[HideInInspector] public int Level; // 레벨

    // 주인공 능력치.
    // 워3 블레이드 마스터 참조.
    public int HitPoints
    {
        get
        {
            return hitPoints;
        }
        set
        {
            // 아이템값 때문에 Max가 저게 아님 변경필요
            hitPoints = Mathf.Clamp(value, 0, UtillFunc.Instance.GetHitPoints(Level));
        }
    }
    private int hitPoints;

    [HideInInspector] public int hasGold; 
	[HideInInspector] public int totalEXP;	// 경험치 총량
	[HideInInspector] public int attackCount;	// 총알발사 카운트
	[HideInInspector] public int ultiGauge;	// 궁극기 게이지

    [HideInInspector] public bool hasKey;


    Dictionary<AbilityType, int> itemAddAbility = new Dictionary<AbilityType, int>();
    Dictionary<AbilityType, int> itemProductAbility = new Dictionary<AbilityType, int>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame()
    {
        // 캐릭터 초기화.
        //if (character == null)
        //	character = Instantiate(objChacater, spawnPoint.position, Quaternion.identity).GetComponent<Character>();
        InitializeAbilityInfo();

		SettingGameInfoText ();
		totalEXP = 0;

		// 몬스터 초기화.
		MonsterSummons.instance.InitMonsters();
		MonsterSummons.instance.SummonsMonster ();

        if(inventory == null)
            inventory = new List<ItemData>();

        inventory.Clear(); 

        CloseShop();
    }

    void InitializeAbilityInfo()
    {
        itemAddAbility.Clear();
        itemProductAbility.Clear();
        SettingCharacterInfo(1);
    }

    public void RestartGame()
    {
        character.transform.position = spawnPoint.position;
    }

	public void SettingCharacterInfo(int lv)
	{
		Level = lv;
		HitPoints = UtillFunc.Instance.GetHitPoints(lv);
	}

    public void FullPoints()
    {
        HitPoints = UtillFunc.Instance.GetHitPoints(Level); 
    }

	public void SettingGameInfoText()
	{
		gameInfoText.text = string.Format ("레벨:{0}\n공격력:{1}-{2}\n방어력:{3}\n총경험치:{4}", 
			Level, UtillFunc.Instance.GetMinAttack(Level), UtillFunc.Instance.GetMaxAttack(Level), UtillFunc.Instance.GetArmor(Level), totalEXP);
	}

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.U))
        {
            HitPoints -= 100;
        }
	}

	public int GetItemAddValue(AbilityType type)
	{
		if (itemAddAbility.ContainsKey(type))
			return itemAddAbility[type];
		return 0;
	}
	public int GetItemProductValue(AbilityType type)
	{
		if (itemProductAbility.ContainsKey(type))
			return itemProductAbility[type];
		return 1;
	}

    public void AquireItem(ItemData itemData)
    {
        inventory.Add(itemData);
        // 조회로 할건지 적용으로 할건지 결정 및 그에 따라서 함수이름 변경
        AdjustItemAbility(itemData);
        UpdateInventoryInfo();
    }

    void AdjustItemAbility(ItemData itemData)
    {
        if (itemData.abilityTypeData.calcurateType == CalcurateType.Sum)
        {
            if(itemAddAbility.ContainsKey(itemData.abilityTypeData.abilityType))
            {
                itemAddAbility[itemData.abilityTypeData.abilityType] += itemData.param; 
            }
            else
            {
                itemAddAbility.Add(itemData.abilityTypeData.abilityType, itemData.param);
            }
        }
        else if (itemData.abilityTypeData.calcurateType == CalcurateType.Product)
        {
            if (itemProductAbility.ContainsKey(itemData.abilityTypeData.abilityType))
            {
                itemProductAbility[itemData.abilityTypeData.abilityType] += itemData.param;
            }
            else
            {
                itemProductAbility.Add(itemData.abilityTypeData.abilityType, itemData.param);
            }
        }
    }

    void AdjustItemAbility(AbilityType abilityType)
    {
        if (abilityType == AbilityType.Specific)
        {
            //id
        }
        else
        {
            int addValue = 0;
            int productValue = 0;
            for (int i = 0; i < inventory.Count; ++i)
            {
                if (abilityType == inventory[i].abilityTypeData.abilityType)
                {
                    if (inventory[i].abilityTypeData.calcurateType == CalcurateType.Sum)
                    {
                        addValue += inventory[i].param;
                    }
                    else if (inventory[i].abilityTypeData.calcurateType == CalcurateType.Product)
                    {
                        productValue += inventory[i].param;
                    }
                }
            }
            itemAddAbility.Add(abilityType, addValue);
            itemProductAbility.Add(abilityType, productValue);
        }
    }

    public bool UseItem(string item)
    {
        return false;
        //int findIndex = inventory.FindIndex(e => e == item);
        //if (findIndex >= 0)
        //{
        //    inventory.RemoveAt(findIndex);
        //    UpdateInventoryInfo();
        //}
        //return (findIndex >= 0);
    }

    void UpdateInventoryInfo()
    {
        for (int i = 0; i < inventoryDisplay.Count; ++i)
        {
            if(i >= inventory.Count)
            {
                inventoryDisplay[i].gameObject.SetActive(false);
            }
            else
            {
                inventoryDisplay[i].gameObject.SetActive(true);
                inventoryDisplay[i].Set(inventory[i]); 
            }
        }
    }

    public void OpenShop()
    {
        shopManager.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        shopManager.gameObject.SetActive(false);
    }

    public void BuyItem(ItemData itemData)
    {
        hasGold -= itemData.price; 
        AquireItem(itemData);
        shopManager.BuyItem(itemData);
    }

	public void SetDamageText(Vector3 pos, string text, Color color)
	{
		bool isSet = false;
		for (int i = 0; i < listDamageText.Count; i++) {
			if (listDamageText [i].gameObject.activeSelf == false) {
				listDamageText [i].SetText (pos, text, color);
				lastDamageText = i;
				isSet = true;
				break;
			}
		}
		if (!isSet) {
			lastDamageText += 1;
			if (listDamageText.Count <= lastDamageText) {
				lastDamageText = 0;
			}
			listDamageText [lastDamageText].SetText (pos, text, color);
		}
	}
}
