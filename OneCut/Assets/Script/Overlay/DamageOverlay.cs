using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : OverlayObject
{
	private float m_fTime;
	private float m_fSpeed = 50f;
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.activeInHierarchy == true) {
			if (m_fTime >= 1f) {
				m_fTime = 0f;
				this.gameObject.SetActive (false);
			}
			this.transform.Translate (0f, Time.deltaTime * m_fSpeed, 0f);
			m_fTime += Time.deltaTime;
		}
	}

	public void SetText(Vector3 pos, string text, Color color, float speed = 50f)
	{
		this.GetComponent<Text> ().text = text;
		this.GetComponent<Text> ().color = color;
        this.transform.position = UtillFunc.Instance.ConvertToUIPosition(pos);
		m_fSpeed = speed;
		m_fTime = 0f;
		this.gameObject.SetActive (true);
	}
}
