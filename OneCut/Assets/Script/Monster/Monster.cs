using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour 
{
	enum eMoveState
	{
		eNormal,
		eTracking,
		eEscape,
		eDie,
	}

	[HideInInspector] public int m_nLevel = 0;
	[HideInInspector] public int m_nLife = 1;
	public Transform imgLife;

    public Vector3 headUpPosition
    {
        get{
            return this.transform.position + new Vector3(0, 1.0f, 0);
        }
    }

	private float m_fSpeedRandomTime;
	private float m_fDirectionRandomTime;
	private bool m_bDirection = true;
	private float m_fSpeed = 1f;
	private int m_nFrameCnt = 0;
	private eMoveState m_eMove = eMoveState.eNormal;	// 이동 패턴
	private Vector3 m_vecCharacter;

	void Start()
	{
		imgLife.GetComponent<SpriteRenderer> ().sortingOrder = 1;
	}

	void OnEnable()
	{
		m_bDirection = Random.Range (0, 100) % 2 == 0;
		this.GetComponent<Animator> ().ResetTrigger ("die");
		this.GetComponent<Animator> ().ResetTrigger ("damage");
		this.GetComponent<Animator> ().ResetTrigger ("idle");
		this.GetComponent<Animator> ().SetTrigger ("idle");

		this.GetComponent<BoxCollider2D>().enabled = true;

		m_eMove = eMoveState.eNormal;
	}

	void OnDisable()
	{
		// 죽음. 데이터 초기화.
		m_nLevel = 0;
		m_nLife = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Move 업데이트.
		UpdateMove ();
		
		// UI 업데이트. 2프레임당 1회.
		if (m_nFrameCnt != Time.frameCount) 
		{
			m_nFrameCnt = Time.frameCount;
			if (m_nFrameCnt % 2 == 0) 
			{
				UpdateUI ();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll) 
	{
		if (coll.gameObject.tag == "weapon") 
		{
			int nLevel = GameManager.instance.Level;
			int nAttack = Random.Range (UtillFunc.Instance.GetMinAttack (nLevel), UtillFunc.Instance.GetMaxAttack (nLevel) + 1);
			//Debug.Log (string.Format("level:{0} attack:{1}",nLevel,nAttack));
			nAttack += GameManager.instance.GetItemAddValue (AbilityType.Attack);
			float itemProduct = GameManager.instance.GetItemProductValue (AbilityType.Attack) / 100f;
			if (itemProduct <= 1f) {
				// 아이템으로 증가되는 능력이 1배이하면 낄필요가없음..
				itemProduct = 1f;
			}
			nAttack = System.Convert.ToInt32 (nAttack * itemProduct);
			Damage (nAttack);
		}
	}

	private void UpdateMove()
	{
		m_fSpeedRandomTime += Time.deltaTime;
		m_fDirectionRandomTime += Time.deltaTime;

		if (m_fSpeedRandomTime >= 0.5f) {
			m_fSpeedRandomTime = 0;
			m_fSpeed = Random.Range (0.1f, 2f);
		}

		switch (m_eMove) {
		case eMoveState.eNormal:
			if (m_fDirectionRandomTime >= 2.5f) {
				m_fDirectionRandomTime = 0;
				m_bDirection = Random.Range (0, 100) % 2 == 0;
			}
			break;
		case eMoveState.eEscape:
			m_vecCharacter = GameManager.instance.character.transform.position;
			// TRUE: 오른쪽이동, FALSE: 왼쪽이동
			if (transform.position.x > m_vecCharacter.x) {
				m_bDirection = true;
			} else if (transform.position.x < m_vecCharacter.x) {
				m_bDirection = false;
			} else {
				m_bDirection = Random.Range (0, 100) % 2 == 0;
			}
			break;
		case eMoveState.eTracking:
			m_vecCharacter = GameManager.instance.character.transform.position;
			// TRUE: 오른쪽이동, FALSE: 왼쪽이동
			if (transform.position.x > m_vecCharacter.x) {
				m_bDirection = false;
			} else if (transform.position.x < m_vecCharacter.x) {
				m_bDirection = true;
			} else {
				m_bDirection = Random.Range (0, 100) % 2 == 0;
			}
			break;
		case eMoveState.eDie:
			break;
		}

		if (m_eMove != eMoveState.eDie) {
			transform.Translate ((m_bDirection ? m_fSpeed : -m_fSpeed) * Time.deltaTime, 0, 0);
		}

		// 밖으로 못나가게 하기.
		if (transform.localPosition.x <= 10f) {
			transform.localPosition = new Vector3 (10f, transform.localPosition.y, 0f);
		}
		else if (transform.localPosition.x >= 25f) {
			transform.localPosition = new Vector3 (25f, transform.localPosition.y, 0f);
		}
	}

	private void UpdateMoveState(float rateHitpoint)
	{
		if (m_eMove != eMoveState.eDie) {
			m_vecCharacter = GameManager.instance.character.transform.position;
			float fDistance = Mathf.Abs (transform.position.x - m_vecCharacter.x);
			if (fDistance <= 3f) {
				if (rateHitpoint >= 0.3f) {
					// 체력이 30%이상이면 공격.
					m_eMove = eMoveState.eTracking;
				} else {
					// 아니면 도망
					m_eMove = eMoveState.eEscape;
				}
			} else {
				// 사거리 밖이면 걍 노멀
				m_eMove = eMoveState.eNormal;
			}
		}
	}

	public void Damage(int damage)
	{
		int nDamage = 0;
		if (damage > 0) {
			// 데미지 감소율 계산. 방어력.
			nDamage = UtillFunc.Instance.GetMonsterDamageReduction (m_nLevel, damage);
			Debug.Log (string.Format("# 공격:{0} 실제데미지:{1}",damage,nDamage));
			m_nLife -= nDamage;
		}

		int minDamage = UtillFunc.Instance.GetMinAttack (GameManager.instance.Level);
		Color color = new Color (1f, 1f, 1f, 0.7f);
		if (minDamage * 2 <= nDamage) {
			color = new Color (1f, 0f, 0f, 0.9f);
		}

		GameManager.instance.SetDamageText(headUpPosition, nDamage.ToString(), color);

		if (m_nLife <= 0) {
			m_nLife = 0;
			Die ();
		} 
		else {
			// 몬스터 피해 연출.
			this.GetComponent<Animator>().ResetTrigger("damage");
			this.GetComponent<Animator>().SetTrigger("damage");
		}
	}

	public void Die()
	{
		m_eMove = eMoveState.eDie;

		GameManager.instance.character.SendMessage ("Exp", m_nLevel);
		//this.gameObject.SetActive (false);

		// 몬스터 죽음 연출.
		this.GetComponent<BoxCollider2D>().enabled = false;
		this.GetComponent<Animator>().ResetTrigger("die");
		this.GetComponent<Animator>().SetTrigger("die");

		// 경험치 획득 텍스트
		int getExp = UtillFunc.Instance.GetMonsterExp (m_nLevel);
		GameManager.instance.SetExpText(headUpPosition, string.Format("exp\n{0}",getExp), new Color(0.5f,0.5f,0.5f,0.5f));
	}

	public void Hide()
	{
		this.gameObject.SetActive (false);
	}

	public void UpdateUI()
	{
		// 생명력 업데이트
		int maxHitpoint = UtillFunc.Instance.GetMonsterLife(m_nLevel);
		float rateHitpoint = (m_nLife*1f) / (maxHitpoint*1f);
		imgLife.localScale = new Vector3 (rateHitpoint * 8f, 1f, 1f);

		UpdateMoveState (rateHitpoint);
	}

	// 몬스터 외형설정. 몬스터가 탄생할때 최초 한번만 불려야함.
	public void SettingGraphic(int level)
	{
		if (m_nLevel == level)
			return;

		m_nLevel = level;
		m_nLife = UtillFunc.Instance.GetMonsterLife(m_nLevel);

		// 몬스터레벨: 캐릭터 레벨 ~ +5 랜덤.
		int nColor = m_nLevel % 10;	// 색은 10종류.
		int nSprite = m_nLevel/20;	// 이미지는 6종류.

		if (nSprite == 0) {
			// 1-19레벨
			//this.GetComponent<SpriteRenderer> ().sprite = null;	// 이미지 변경.
		} else if (nSprite == 1) {
			// 20-39레벨
		} else if (nSprite == 2) {
			// 40-59레벨
		} else if (nSprite == 3) {
			// 60-79레벨
		} else if (nSprite == 4) {
			// 80-99레벨
		} else {
			// 100레벨 이상
		}

		if (nColor == 0) {
			this.GetComponent<SpriteRenderer> ().color = Color.white;
		} else if (nColor == 1) {
			this.GetComponent<SpriteRenderer> ().color = new Color (0f, 0.5f, 0f);
		} else if (nColor == 2) {
			this.GetComponent<SpriteRenderer> ().color = new Color (0f, 0.8f, 0f);
		} else if (nColor == 3) {
			this.GetComponent<SpriteRenderer> ().color = new Color (0f, 0.5f, 0.5f);
		} else if (nColor == 4) {
			this.GetComponent<SpriteRenderer> ().color = new Color (0f, 0.8f, 0.8f);
		} else if (nColor == 5) {
			this.GetComponent<SpriteRenderer> ().color = new Color (0f, 0f, 0.5f);
		} else if (nColor == 6) {
			this.GetComponent<SpriteRenderer> ().color = new Color (0f, 0f, 0.8f);
		} else if (nColor == 7) {
			this.GetComponent<SpriteRenderer> ().color = new Color (0.5f, 0f, 0f);
		} else if (nColor == 8) {
			this.GetComponent<SpriteRenderer> ().color = new Color (0.8f, 0f, 0f);
		} else if (nColor == 9) {
			this.GetComponent<SpriteRenderer> ().color = new Color (0.8f, 0.8f, 0.8f);
		}
	}
}
