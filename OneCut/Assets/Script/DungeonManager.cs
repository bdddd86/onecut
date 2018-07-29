using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaynomicsPlugin;

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

public enum eMissileDirection	// 미사일 공격 방향
{
	eNull = -1,
	eLeftRight = 0,
	eRightLeft,
	eTopBottom,
	eBottomTop,
}

public class AttackData
{
	public eAttackType type;	// 공격종류
	public float delay;			// 선 딜레이
	// 광역 폭발
	public Vector2 org;			// 폭발원점(던전 공간 기준)
	public float distance;		// 영향범위(x축)
	public int effect_type;		// 이팩트 종류
	// 미사일 공격
	public eMissileDirection dir_type;	// 미사일방향
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
	public AttackData(float delay, eMissileDirection dir_type, float speed, int prefab_type)
	{
		type = eAttackType.eMissile;
		this.delay = delay;
		this.dir_type = dir_type;
		this.speed = speed;
		this.prefab_type = prefab_type;
	}
}

public class DungeonManager : MonoSingleton<DungeonManager> {

	public GameObject areaWanning;	// 광역공격 지역 표시.

	private ConcurrentQueue<AttackData> queueTempo = new ConcurrentQueue<AttackData>();
	private AttackData currentData = null;

	void Start()
	{
		CreatePattern ();
	}

	public void CreatePattern()
	{
		// 공격패턴 1.
		queueTempo.Enqueue(new AttackData(1.5f, new Vector2(0, 0), 5f, 0));
		queueTempo.Enqueue(new AttackData(3f));	// 3초 쉬고...
		queueTempo.Enqueue(new AttackData(1.5f, new Vector2(0, 0), 2f, 0));
		queueTempo.Enqueue (new AttackData ());	// 마지막 표시
	}

	public void Attack()
	{
		if (currentData == null) {
			currentData = queueTempo.Dequeue ();
			if (currentData.type == eAttackType.eTempoEnd) {
				// 공격 종료. 다음 호출대기.
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
		if (currentData.type == eAttackType.eAreaEx) {
			// 광역 공격 위치 표시.
			areaWanning.gameObject.SetActive(true);
			areaWanning.transform.position = currentData.org;
			areaWanning.transform.localScale = new Vector3(currentData.distance*2f, 100f, 1f);
			Debug.Log(string.Format("*광역공격 위치: X:{0}, D:{1}",currentData.org.x, currentData.distance));
		}
		yield return new WaitForSeconds (currentData.delay);
		areaWanning.gameObject.SetActive(false);
		if (currentData.type == eAttackType.eDelay) {
			// 딜레이만 줌.
		}
		// 광역공격 들어감.
		else if (currentData.type == eAttackType.eAreaEx){
			GameManager.instance.character.OnRecvAreaAttack (currentData.org, currentData.distance, 100);
			// 폭발 이팩트.
			Debug.Log (string.Format ("*폭발이팩트 애니메이션 하기. {0}", currentData.effect_type));
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
