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

	public float distance;		// 영향범위(x축)

	public Vector3 start_pos;
	public Vector3 end_pos;
	public float speed;			// 미사일 속도

	public AttackData()
	{
		type = eAttackType.eTempoEnd;
	}
	public AttackData(float delay)
	{
		type = eAttackType.eDelay;
		this.delay = delay;
	}

	public AttackData(float delay, float distance)
	{
		type = eAttackType.eAreaEx;
		this.delay = delay;
		this.distance = distance;
	}

	public AttackData(float delay, Vector3 start, Vector3 end, float speed)
	{
		type = eAttackType.eMissile;
		this.delay = delay;
		this.start_pos = start;
		this.end_pos = end;
		this.speed = speed;
	}
}
/*
[Serializable]
public class AreaAttackData : AttackData
{
	public float distance;		// 영향범위(x축)
	public AreaAttackData(float delay, float distance)
	{
		type = eAttackType.eAreaEx;
		this.delay = delay;
		this.distance = distance;
	}
}
[Serializable]
public class MissileAttackData : AttackData
{
	public Vector3 start_pos;
	public Vector3 end_pos;
	public float speed;			// 미사일 속도
	public MissileAttackData(float delay, Vector3 start, Vector3 end, float speed)
	{
		type = eAttackType.eMissile;
		this.delay = delay;
		this.start_pos = start;
		this.end_pos = end;
		this.speed = speed;
	}
}
*/

[Serializable]
public class AttackPattern
{
	public int ID;
	public List<AttackData> listAttackData;

	public AttackPattern(int id, List<AttackData> list)
	{
		ID = id;
		listAttackData = new List<AttackData> (list);
	}
}

public class DungeonManager : MonoSingleton<DungeonManager> {

	public Transform m_orgin;	// 기준점
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

		Test_CreatePattern ();
	}

	void Test_CreatePattern()
	{
		queueTempo.Enqueue (new AttackData (0.5f, new Vector3(0f, 2f, 0f), new Vector3 (-5f, 3f, 0f), 0.1f));
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
				AttackData data = currentData;
				m_imgAreaWanning.gameObject.SetActive (true);
				m_imgAreaWanning.transform.position = m_orgin.position;
				m_imgAreaWanning.transform.localScale = new Vector3 (data.distance * 2f, 100f, 1f);
				Debug.Log (string.Format ("*광역공격 위치: X:{0}, D:{1}", m_orgin.position.x, data.distance));
			}
			break;
		case eAttackType.eMissile:
			{
				AttackData data = currentData;
				for (int i = 0; i < m_listMissile.Count; i++) {
					if (m_listMissile [i].gameObject.activeSelf == false) {
						missile = m_listMissile [i];
						break;
					}
				}
				missile.startPos = data.start_pos;
				missile.endPos = data.end_pos;
				missile.speed = data.speed;
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
				AttackData data = currentData;
				m_imgAreaWanning.gameObject.SetActive(false);

				GameManager.instance.character.OnRecvAreaAttack (m_orgin.position, data.distance, 100);
				// 폭발 이팩트.
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