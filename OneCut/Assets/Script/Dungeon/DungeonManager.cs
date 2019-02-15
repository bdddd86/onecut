﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoSingleton<DungeonManager> {

	private int m_nLevel = 0;
	public int DungeonLevel{
		set{
			if (m_nLevel != value) {
				m_nLevel = value;
				onChangeLevel ();
			}
		}
		get{
			return m_nLevel;
		}
	}

	public Transform m_rootFire;
	private List<Fire> m_listFire = new List<Fire> ();

	void Start()
	{
		for (int i = 0; i < m_rootFire.childCount; i++) {
			Transform child = m_rootFire.GetChild (i);
			if (child != null && child.GetComponent<Fire> () != null) {
				m_listFire.Add (child.GetComponent<Fire> ());
			}
		}
		for (int i = 0; i < m_listFire.Count; i++) {
			m_listFire [i].transform.localPosition = new Vector3 (-8f + i, 6f, 0f);
		}
	}

	private void onChangeLevel()
	{
		
	}

	// 불 공격 시작.
	public void onFire()
	{
		for (int i = 0; i < m_listFire.Count; i++) {
			m_listFire [i].gameObject.SetActive (false);
		}

		// 셔플
		UtillFunc.Instance.ShuffleList<Fire> (ref m_listFire);

		int nFireCnt = UtillFunc.Instance.GetDungeonFireCount (DungeonLevel);
		for (int i = 0; i < m_listFire.Count; i++) {
			Vector3 vec = m_listFire [i].transform.localPosition;
			m_listFire [i].transform.localPosition = new Vector3 (vec.x, 6f, 0f);
			m_listFire [i].gameObject.SetActive (i < nFireCnt);
		}
	}
}
