using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

	private Vector3 m_vStart;
	private Vector3 m_vEnd;
	private Vector3 m_vHeight;
	private float m_fPercent;
	public float speed;
	public Animator m_aniExplosion;
	//public float lifeTime;
	//public float startTime;

	public void Shot(Vector3 start, Vector3 height, Vector3 end)
	{
		m_vStart = start;
		m_vEnd = end;
		m_vHeight = height;
		m_fPercent = 0;
		m_aniExplosion.gameObject.SetActive (false);
		m_aniExplosion.Rebind ();

		this.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.activeInHierarchy == true) 
		{
			m_fPercent += Time.deltaTime * speed;
			if (m_fPercent >= 1f) {
				m_fPercent = 1f;
			}
			this.transform.localPosition = BezierCurve (m_fPercent, m_vStart, m_vHeight, m_vEnd);

			//if (Time.time - startTime >= lifeTime || m_fPercent >= 1f) 
			if (m_fPercent >= 1f)
			{
				this.gameObject.SetActive (false);
				m_fPercent = 0f;

				// 폭발일으킴.
				Debug.Log("폭발");
				m_aniExplosion.transform.localPosition = this.transform.localPosition;
				m_aniExplosion.gameObject.SetActive (true);
				m_aniExplosion.Play ("explosion");

				int nLevel = GameManager.instance.Level;
				int nAttack = Random.Range (UtillFunc.Instance.GetMinAttack (nLevel), UtillFunc.Instance.GetMaxAttack (nLevel) + 1);
				nAttack += GameManager.instance.GetItemAddValue (AbilityType.Attack);	// 아이템 추가공격력
				nAttack = System.Convert.ToInt32(nAttack * (GameManager.instance.GetItemProductValue(AbilityType.Attack)/100f));	// 아이템 추가%
				// 폭탄공격은 크리티컬없음
				MonsterSummons.instance.OnRecvAreaAttack (this.transform.localPosition, 3f, nAttack * 5);
			}
		}
	}

	// 베지어 곡선.
	Vector3 BezierCurve( float t, Vector3 p0 , Vector3 p1 ) {
		return ( ( 1 - t  ) * p0 ) + ( ( t  ) * p1 );
	}

	Vector3 BezierCurve( float t, Vector3 p0 , Vector3 p1 , Vector3 p2 ) {
		Vector3 pa = BezierCurve( t , p0, p1 );
		Vector3 pb = BezierCurve( t , p1, p2 );
		return BezierCurve( t , pa , pb );
	}
}
