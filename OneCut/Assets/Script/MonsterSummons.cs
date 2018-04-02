using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSummons : MonoSingleton<MonsterSummons> {

	public int m_nDungeonLevel = 1;
	[HideInInspector] public List<Monster> m_listMonsters = new List<Monster> ();

	void Start()
	{
		m_listMonsters.Clear ();

		for (int i = 0; i < transform.childCount; i++) {
			m_listMonsters.Add(transform.GetChild (i).GetComponent<Monster>());
		}
	}

	public void SummonsMonster()
	{
		int activeCnt = 0;
		for (int i = 0; i < m_listMonsters.Count; i++) {
			if (m_listMonsters [i].gameObject.activeInHierarchy == false) {
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
