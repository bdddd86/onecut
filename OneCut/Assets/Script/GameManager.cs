using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> {

    public Transform spawnPoint;

    public GameObject objChacater;
    public GameObject blackScreen;

	public Character character;

	[HideInInspector] public int m_nLevel;	// 레벨

	// 스탯값. 레벨에따라 오르고 버프나 장착아이템으로 증가가능.
	[HideInInspector] public int m_nPow;	// 힘. 체력, 체력재생, 공격력
	[HideInInspector] public int m_nDex;	// 민첩. 방어력, 공격속도, 이동속도
	[HideInInspector] public int m_nInc;	// 지능. 스플레쉬 어택, 경험치 획득량, 크리티컬 확률(약점)

	// 2차 스탯 값. 힘,민,지로 계산되는 능력치.
	[HideInInspector] public int m_nLife;	// 현재 체력
	[HideInInspector] public int m_nMaxLife;	// 총 체력
	[HideInInspector] public float m_fRegenLife;	// 체력 재생력
	[HideInInspector] public float m_fDamage;	// 공격력
	[HideInInspector] public float m_fDefence;	// 방어력
	[HideInInspector] public float m_fAttackSpeed;	// 공격속도(기본1)
	[HideInInspector] public float m_fMoveSpeed;	// 이동속도(기본1)
	[HideInInspector] public float m_fSplashArea;	// 공격 범위
	[HideInInspector] public float m_nExp;	// 현재 경험치
	[HideInInspector] public float m_fAddExp;	// 추가 경험치
	[HideInInspector] public float m_fCritical;	// 크리티컬 확률

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame()
    {
		// 캐릭터 초기화.
		if (character == null)
        	character = Instantiate(objChacater, spawnPoint.position, Quaternion.identity).GetComponent<Character>();
		SettingCharacterInfo (1);
		m_nExp = 0;

		// 몬스터 초기화.
		MonsterSummons.instance.InitMonsters();
		MonsterSummons.instance.SummonsMonster ();
    }

    public void RestartGame()
    {
        character.transform.position = spawnPoint.position;
    }

	public void SettingCharacterInfo(int level)
	{
		m_nLevel = level;
		m_nPow = UtillFunc.Instance.pow (m_nLevel);
		m_nDex = UtillFunc.Instance.dex (m_nLevel);
		m_nInc = UtillFunc.Instance.inc (m_nLevel);
		m_nMaxLife = UtillFunc.Instance.maxLife (m_nPow);
		m_fRegenLife = UtillFunc.Instance.regenLife (m_nPow);
		m_fDamage = UtillFunc.Instance.attackDamage (m_nPow);
		m_fDefence = UtillFunc.Instance.defence (m_nDex);
		m_fAttackSpeed = UtillFunc.Instance.attackSpeed (m_nDex);
		m_fMoveSpeed = UtillFunc.Instance.moveSpeed (m_nDex);
		m_fSplashArea = UtillFunc.Instance.splashArea (m_nInc);
		m_fAddExp = UtillFunc.Instance.addExp (m_nInc);
		m_fCritical = UtillFunc.Instance.critical (m_nInc);
		m_nLife = m_nMaxLife;
		//m_fExp = 0;
	}

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
