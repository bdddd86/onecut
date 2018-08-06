using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaynomicsPlugin;
using System;

public enum eDungeonState
{
	eNull = -1,
	eNormal = 0,	// 일반 던젼
	eBoss,			// 보스전
}

public enum eAttackType
{
	eNull = -1,
	eDelay = 0,		// 쉬어가는 곳
	eAreaEx,		// 광역 폭발
	eMissile,		// 미사일
	eTempoEnd,		// 템포 공격 마지막
}

[Serializable]
public class AttackData
{
	public eAttackType type;	// 공격종류
	public float delay;			// 선 딜레이
	// 광역 폭발
	[Header("[Area Ex]")]
	public Vector2 org;			// 폭발원점(던전 공간 기준)
	public float distance;		// 영향범위(x축)
	public int effect_type;		// 이팩트 종류
	// 미사일 공격
	[Header("[Missile]")]
	public Vector3 start_pos;
	public Vector3 end_pos;
	public float speed;			// 미사일 속도
	public int prefab_type;		// 미사일 프리팹 종류

	public AttackData()
	{
		type = eAttackType.eTempoEnd;
	}
	public AttackData(float delay)
	{
		type = eAttackType.eDelay;
		this.delay = delay;
	}
	public AttackData(float delay, Vector2 org, float distance, int effect_type)
	{
		type = eAttackType.eAreaEx;
		this.delay = delay;
		this.org = org;
		this.distance = distance;
		this.effect_type = effect_type;
	}
	public AttackData(float delay, Vector3 start, Vector3 end, float speed, int prefab_type)
	{
		type = eAttackType.eMissile;
		this.delay = delay;
		this.start_pos = start;
		this.end_pos = end;
		this.speed = speed;
		this.prefab_type = prefab_type;
	}
}

[Serializable]
public class AttackPattern
{
	public int ID;
	public List<AttackData> listAttackData;

	public AttackPattern(int id, List<AttackData> list)
	{
		ID = id;
		listAttackData = list;
	}
}

public class DungeonManager : MonoSingleton<DungeonManager> {

	public GameObject m_imgAreaWanning;	// 광역공격 지역 표시.
	public List<DungeonMissile> m_listMissile;	// 미사일 리스트
	[Header("[패턴등록]")]
	public List<AttackPattern> patternData;

	private Dictionary<int, AttackPattern> m_dicPattern = new Dictionary<int, AttackPattern> ();
	private ConcurrentQueue<AttackData> queueTempo = new ConcurrentQueue<AttackData>();
	private AttackData currentData = null;

	void Start()
	{
		m_dicPattern.Clear ();

		for (int i = 0; i < patternData.Count; i++) {
			if (m_dicPattern.ContainsKey (patternData [i].ID))
				continue;
			m_dicPattern.Add (patternData [i].ID, new AttackPattern(patternData[i].ID, patternData[i].listAttackData));
		}

		CreatePattern (102);
		CreatePattern (101);
	}

	public void CreatePattern(int id)
	{
		if (m_dicPattern.ContainsKey (id) == false) {
			return;
		}

		for(int i=0; i<m_dicPattern[id].listAttackData.Count; i++)
		{
			queueTempo.Enqueue (m_dicPattern [id].listAttackData [i]);
		}
	}

	public void Attack()
	{
		if (currentData == null) {
			currentData = queueTempo.Dequeue ();
			if (currentData.type == eAttackType.eTempoEnd) {
				// 공격 종료. 다음 호출대기.
				currentData = null;
				Debug.Log("공격 종료. 다음 공격 대기중");
			} 
			else {
				StartCoroutine (OnAttack ());
			}
		}
		else {
			// 진행중인 공격템포가 있음.
			Debug.Log("공격 진행중...");
		}
	}

	IEnumerator OnAttack()
	{
		// 공격 시작 동작.
		DungeonMissile missile = null;

		switch (currentData.type) {
		case eAttackType.eAreaEx: 
			{
				// 광역 공격 위치 표시.
				m_imgAreaWanning.gameObject.SetActive (true);
				m_imgAreaWanning.transform.position = currentData.org;
				m_imgAreaWanning.transform.localScale = new Vector3 (currentData.distance * 2f, 100f, 1f);
				Debug.Log (string.Format ("*광역공격 위치: X:{0}, D:{1}", currentData.org.x, currentData.distance));
			}
			break;
		case eAttackType.eMissile:
			{
				for (int i = 0; i < m_listMissile.Count; i++) {
					if (m_listMissile [i].gameObject.activeSelf == false) {
						missile = m_listMissile [i];
						break;
					}
				}
				missile.startPos = currentData.start_pos;
				missile.endPos = currentData.end_pos;
				missile.speed = currentData.speed;
				missile.transform.position = missile.startPos;
			}
			break;
		}
		yield return new WaitForSeconds (currentData.delay);
		// 공격 딜레이 이후 동작.
		switch(currentData.type)
		{
		case eAttackType.eAreaEx:
			{
				m_imgAreaWanning.gameObject.SetActive(false);

				GameManager.instance.character.OnRecvAreaAttack (currentData.org, currentData.distance, 100);
				// 폭발 이팩트.
				Debug.Log (string.Format ("*폭발이팩트 애니메이션 하기. {0}", currentData.effect_type));
			}
			break;
		case eAttackType.eDelay:
			{
			}
			break;
		case eAttackType.eMissile:
			{
				// 미사일 발사 시키기.
				missile.gameObject.SetActive(true);
			}
			break;
		}

		yield return null;

		CheckNextAttack ();
	}

	public void CheckNextAttack()
	{
		if (queueTempo.Count > 0) {
			currentData = null;
			this.Attack ();
		}
	}
}