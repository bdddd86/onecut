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

    List<string> inventory; 

	//[HideInInspector] public Character character;

	[HideInInspector] public int Level;	// 레벨

	// 주인공 능력치.
	// 워3 블레이드 마스터 참조.
	[HideInInspector] public int HitPoints;	// 체력
	[HideInInspector] public int totalEXP;	// 경험치 총량
	[HideInInspector] public int attackCount;	// 총알발사 카운트
	[HideInInspector] public int ultiGauge;	// 궁극기 게이지

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
		SettingCharacterInfo (1);
		SettingGameInfoText ();
		totalEXP = 0;

		// 몬스터 초기화.
		MonsterSummons.instance.InitMonsters();
		MonsterSummons.instance.SummonsMonster ();

        if(inventory == null)
            inventory = new List<string>();

        inventory.Clear(); 

        CloseShop();
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
		
	}

    public void AquireItem(string item)
    {
        inventory.Add(item);
        UpdateInventoryInfo();
    }

    public bool UseItem(string item)
    {
        int findIndex = inventory.FindIndex(e => e == item);
        if (findIndex >= 0)
        {
            inventory.RemoveAt(findIndex);
            UpdateInventoryInfo();
        }
        return (findIndex >= 0);
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

}
