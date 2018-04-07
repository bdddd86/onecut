using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSummons : MonoSingleton<MonsterSummons> {

	[HideInInspector] public List<Monster> m_listMonsters = new List<Monster> ();

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

	public void SummonsMonster()
	{
		int activeCnt = 0;
		for (int i = 0; i < m_listMonsters.Count; i++) {
			if (m_listMonsters [i].gameObject.activeInHierarchy == false) {
				m_listMonsters [i].SettingGraphic (GameManager.instance.m_nLevel);
				m_listMonsters [i].gameObject.SetActive (true);
				activeCnt += 1;
			}
			if (activeCnt == 10) {
				break;
			}
		}
		if (activeCnt < 10) {
			Debug.Log ("max monster.");
		}
	}
}
