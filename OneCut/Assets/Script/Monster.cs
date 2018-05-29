using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

	public int m_nLevel = 0;
	public int m_nLife = 1;

	private float m_fSpeedRandomTime;
	private float m_fDirectionRandomTime;
	private bool m_bDirection = true;
	private float m_fSpeed = 1f;

	void OnEnable()
	{
		m_bDirection = Random.Range (0, 100) % 2 == 0;
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
		m_fSpeedRandomTime += Time.deltaTime;
		m_fDirectionRandomTime += Time.deltaTime;

		if (m_fSpeedRandomTime >= 0.5f) {
			m_fSpeedRandomTime = 0;
			m_fSpeed = Random.Range (0.1f, 2f);
		}

		if (m_fDirectionRandomTime >= 2.5f) {
			m_fDirectionRandomTime = 0;
			m_bDirection = Random.Range (0, 100) % 2 == 0;
		}
		
		transform.Translate ((m_bDirection?m_fSpeed:-m_fSpeed) * Time.deltaTime, 0, 0);

		if (transform.localPosition.x <= 9f) {
			transform.localPosition = new Vector3 (9f, transform.localPosition.y, 0f);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) 
	{
		//if (coll.gameObject.tag == "wall") {
		//	Debug.Log ("monster collision wall");
		//	m_bDirection = !m_bDirection;
		//}

		if (coll.gameObject.tag == "weapon") {
			Debug.Log ("monster collision weapon");
			m_nLife -= 100;
			if (m_nLife <= 0) {
				GameManager.instance.character.SendMessage ("Exp", m_nLevel);
				this.gameObject.SetActive (false);
			}
		}
	}

	// 몬스터 외형설정. 몬스터가 탄생할때 최초 한번만 불려야함.
	public void SettingGraphic(int level)
	{
		if (m_nLevel == level)
			return;

		m_nLevel = level;
		m_nLife = UtillFunc.Instance.monsterLife(m_nLevel);

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
