using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 소환기
public class MonsterSummonManager : MonoSingleton<MonsterSummonManager>
{
	[HideInInspector] public List<Monster> m_listMonsters = new List<Monster> ();
	private int totalSummon = 0;

	void Awake()
	{
		m_listMonsters.Clear ();

		for (int i = 0; i < transform.childCount; i++) {
			m_listMonsters.Add(transform.GetChild (i).GetComponent<Monster>());
		}
	}

	public void InitMonsters()
	{
		for (int i = 0; i < m_listMonsters.Count; i++) {
			m_listMonsters [i].gameObject.SetActive (false);
		}
	}

	public bool SummonsMonster(int summonCnt)
	{
		int activeCnt = 0;
		for (int i = 0; i < m_listMonsters.Count; i++) {
			if (m_listMonsters [i].gameObject.activeInHierarchy == false) {
				m_listMonsters [i].SettingGraphic (GameManager.instance.Level);
				m_listMonsters [i].gameObject.SetActive (true);
				activeCnt += 1;
			}
			if (activeCnt >= summonCnt) {
				break;
			}
		}

		totalSummon = 0;
		for (int i = 0; i < m_listMonsters.Count; i++) {
			if (m_listMonsters [i].gameObject.activeInHierarchy == true) {
				totalSummon += 1;
			}
		}

		if (m_listMonsters.Count <= totalSummon) {
			return false;
		}
		return true;
	}

	// 광역피해 전달. (원점, 범위, 데미지)
	public void OnRecvAreaAttack(Vector3 org, float distance, int damage)
	{
		for (int i = 0; i < m_listMonsters.Count; i++) {
			if (m_listMonsters [i].gameObject.activeInHierarchy == true) {
				if (distance >= Vector3.Distance (m_listMonsters [i].transform.localPosition, org)) {
					m_listMonsters [i].Damage (damage);
				}
			}
		}
	}

	// 전체피해. (데미지)
	public void OnRecvGlobalAttack(int damage)
	{
		for (int i = 0; i < m_listMonsters.Count; i++) {
			if (m_listMonsters [i].gameObject.activeInHierarchy == true) {
				m_listMonsters [i].Damage (damage);
			}
		}
	}
}
