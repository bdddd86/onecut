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

	//public Vector3 start_pos;
	//public Vector3 end_pos;
	public int dir_right;	// 0:사용안함, 1:오른쪽, 2:왼쪽
	public int dir_top;		// 0:사용안함, 1:위쪽, 2:아래쪽
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

	public AttackData(float delay, int dir_right, int dir_top, float speed)
	{
		type = eAttackType.eMissile;
		this.delay = delay;
		//this.start_pos = start;
		//this.end_pos = end;
		this.dir_right = dir_right;
		this.dir_top = dir_top;
		this.speed = speed;
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

		//Test_CreatePattern ();
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
				missile.dir_right = data.dir_right;
				missile.dir_top = data.dir_top;
				missile.speed = data.speed;
				//missile.transform.position = missile.startPos;
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