using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> 
{
    public enum eGameMode
    {
        eHome,
		eEvent,
        eDungeon, 
        eBoss,
    }

    public ShopPopupUIObject shopManager;
    public Transform spawnPoint;

    //public GameObject objChacater;
	public Character character;
    public GameObject blackScreen;

    public List<ItemBuffUIObject> inventoryDisplay; 

	[Header("[game infos]")]
	public Text charLvText;
	public Text charHPText;
	public Text charGoldText;
	public Text charAttackText;
	public Text charArmorText;

	[Header("[monster damage text]")]
	public List<DamageOverlay> listDamageText;
	private int lastDamageText = 0;
	[Header("[exp text]")]
	public List<DamageOverlay> listExpText;
	private int lastExpText = 0;

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
			if (hitPoints != value) {
				// 아이템값 때문에 Max가 저게 아님 변경필요
				hitPoints = Mathf.Clamp (value, 0, UtillFunc.Instance.GetHitPoints (Level));
				SettingGameInfoText ();
			}
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

    [HideInInspector] public eGameMode currentGameMode;
    // mode interface
    public GameObject btnJump;
    public GameObject btnAttack;
    public GameObject btnEvasion; 

    // testCode
    public TestNPC testNPC; 

	// Dungeon
	private float dungeonDartTime = 0f;	// 다트 공격 시간
	private float dungeonFireTime = 0f;	// 불 공격 시간

	// 코루틴 관리
	Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();

	// 게임 시작 시간
	private float gameStartTime = 0f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
		UtillFunc.Instance.init ();
        InitGame();
    }

    void InitGame()
    {
		gameStartTime = 1;

        ChangeGameMode(eGameMode.eHome);

        InitializeAbilityInfo();

		SettingGameInfoText ();
		totalEXP = 0;

		// 몬스터 초기화.
		MonsterSummonManager.instance.InitMonsters();
		MonsterSummonManager.instance.SummonsMonster (10);

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
		int nNextNeed = UtillFunc.Instance.GetNeedExpToNextLv(Level);
		int nCurrent = UtillFunc.Instance.GetTotalExpFromLv (Level + 1) - totalEXP;
		float fRate = System.Convert.ToSingle(nCurrent) / System.Convert.ToSingle(nNextNeed);

		charLvText.text = string.Format ("LV.{0}<size=10>({1:F1}%)</size>",Level,(1f-fRate)*100f);
		charHPText.text = string.Format ("{0}/{1}",HitPoints,UtillFunc.Instance.GetHitPoints(Level));
		charGoldText.text = string.Format ("{0}",hasGold);
		if (GetItemAddValue (AbilityType.Attack) <= 0) {
			charAttackText.text = string.Format ("{0}",UtillFunc.Instance.GetMinAttack(Level));
		} else {
			charAttackText.text = string.Format ("{0}<color=green><size=10>(+{1})</size></color>",UtillFunc.Instance.GetMinAttack(Level),GetItemAddValue(AbilityType.Attack));
		}
		if (GetItemAddValue (AbilityType.Defense) <= 0) {
			charArmorText.text = string.Format ("{0}",UtillFunc.Instance.GetArmor(Level));
		} else {
			charArmorText.text = string.Format ("{0}<color=green><size=10>(+{1})</size></color>",UtillFunc.Instance.GetArmor(Level),GetItemAddValue(AbilityType.Defense));
		}
	}

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(Input.GetKeyDown(KeyCode.U))
        {
            HitPoints -= 100;
        }

		// 게임모드는 캐릭터위치 기반이다.
		// x >= 8.5 던전, x <= -8.5 이벤트
		if (character != null) {
			if (character.transform.localPosition.x >= 8.5f) {
				ChangeGameMode (eGameMode.eDungeon);
			} else if (character.transform.localPosition.x <= -8.5f) {
				ChangeGameMode (eGameMode.eEvent);
			} else {
				ChangeGameMode (eGameMode.eHome);
			}
		}

		// 게임던전은 게임시작 기반으로 자동 성장한다.
		if (gameStartTime != 0f) {
			DungeonManager.instance.ChangeLevel (Time.time - gameStartTime);
		}

		UpdateDungeon ();
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
				listDamageText [i].SetText (pos, text, color, 100f);
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
			listDamageText [lastDamageText].SetText (pos, text, color, 100f);
		}
	}

	public void SetExpText(Vector3 pos, string text, Color color)
	{
		bool isSet = false;
		for (int i = 0; i < listExpText.Count; i++) {
			if (listExpText [i].gameObject.activeSelf == false) {
				listExpText [i].SetText (pos, text, color, 150f);
				lastExpText = i;
				isSet = true;
				break;
			}
		}
		if (!isSet) {
			lastExpText += 1;
			if (listExpText.Count <= lastExpText) {
				lastExpText = 0;
			}
			listExpText [lastExpText].SetText (pos, text, color, 150f);
		}
	}

    public void ChangeGameMode(eGameMode gameMode)
    {
		if (currentGameMode == gameMode) {
			return;
		}
        currentGameMode = gameMode; 
        switch(gameMode)
        {
            case eGameMode.eDungeon:
                SetDungeonMode();
                break; 
            case eGameMode.eHome:
			case eGameMode.eEvent:
                SetHomeMode();
                break;
            case eGameMode.eBoss:
                SetBossMode();
                break;
        }
        testNPC.SetNPCName();
    }

    void SetDungeonMode()
    {
        btnJump.SetActive(false);
        btnAttack.SetActive(true);
        btnEvasion.SetActive(true);
    }

    void SetHomeMode()
    {
        btnJump.SetActive(true);
        btnAttack.SetActive(false);
        btnEvasion.SetActive(false);
    }

    void SetBossMode()
    {
        btnJump.SetActive(false);
        btnAttack.SetActive(true);
        btnEvasion.SetActive(true);
    }

	// 던전 업데이트
	void UpdateDungeon()
	{
		if (currentGameMode == eGameMode.eDungeon) {
			dungeonDartTime += Time.deltaTime;
			dungeonFireTime += Time.deltaTime;

			if (dungeonDartTime >= 5f) {
				dungeonDartTime = 0f;
				DungeonManager.instance.onDart ();
			}
			if (dungeonFireTime >= 3f) {
				dungeonFireTime = 0f;
				DungeonManager.instance.onFire ();
			}
		}
	}

	// 테스트용
	#if false//UNITY_EDITOR
	void OnGUI()
	{
		int nX = 500;
		if (GUI.Button (new Rect (nX+10, 10, 200, 50), "Lazer")) {
			//PlayShot (GameManager.instance.character.IsRight());
			GameManager.instance.character.LazerAttack();
		}
		if (GUI.Button (new Rect (nX+210, 10, 200, 50), "Damage")) {
			GameManager.instance.character.Damage(30);
		}
		if (GUI.Button (new Rect (nX+410, 10, 200, 50), "Dungeon Level")) {
			DungeonManager.instance.DungeonLevel = 6;
		}
		if (GUI.Button (new Rect (nX+10, 60, 200, 50), "Fire")) {
			DungeonManager.instance.onFire();
		}
		if (GUI.Button (new Rect (nX+210, 60, 200, 50), "Dart")) {
			DungeonManager.instance.onDart ();
		}
	}
	#endif
}
