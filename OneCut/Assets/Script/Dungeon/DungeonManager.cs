﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoSingleton<DungeonManager> {

	private int m_nLevel = 0;
	public int DungeonLevel{
		set{
			if (m_nLevel != value) {
				m_nLevel = value;
			}
		}
		get{
			return m_nLevel;
		}
	}

	public Transform m_rootFire;
	private List<Fire> m_listFire = new List<Fire> ();

	public Transform m_rootDart;
	private List<Dart> m_listDart = new List<Dart>();

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

		for (int i = 0; i < m_rootDart.childCount; i++) {
			Transform child = m_rootDart.GetChild (i);
			if (child != null && child.GetComponent<Dart> () != null) {
				m_listDart.Add (child.GetComponent<Dart> ());
			}
		}
	}

	public void ChangeLevel(float gameTime)
	{
		if (gameTime <= 20f) {
			DungeonLevel = 0;
		} else if (gameTime <= 40f) {
			DungeonLevel = 1;
		} else if (gameTime <= 60f) {
			DungeonLevel = 2;
		} else if (gameTime <= 80f) {
			DungeonLevel = 3;
		} else if (gameTime <= 100f) {
			DungeonLevel = 4;
		} else if (gameTime <= 130f) {
			DungeonLevel = 5;
		} else if (gameTime <= 160f) {
			DungeonLevel = 6;
		} else if (gameTime <= 190f) {
			DungeonLevel = 7;
		} else if (gameTime <= 220f) {
			DungeonLevel = 8;
		} else if (gameTime <= 250f) {
			DungeonLevel = 9;
		} else {
			DungeonLevel = 10;
		}
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

	// 다트 공격 시작.
	public void onDart()
	{
		int nDartCnt = UtillFunc.Instance.GetDungeonDartCount (DungeonLevel);

		int nOn = 0;
		for (int i = 0; i < m_listDart.Count; i++) {
			if (m_listDart [i].gameObject.activeSelf == false) {
				nOn += 1;
				if (nDartCnt < nOn) {
					break;
				}
				m_listDart [i].gameObject.SetActive (true);
			}
		}
	}
}
