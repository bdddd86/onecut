using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

	//public Rigidbody2D rigidBody;

	private float m_fSpeedRandomTime;
	private float m_fDirectionRandomTime;
	private bool m_bDirection = true;
	private float m_fSpeed = 1f;

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable()
	{
		m_bDirection = Random.Range (0, 100) % 2 == 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_fSpeedRandomTime += Time.deltaTime;
		m_fDirectionRandomTime += Time.deltaTime;

		if (m_fSpeedRandomTime >= 0.5f) {
			m_fSpeedRandomTime = 0;
			m_fSpeed = Random.Range (0.1f, 2f);
		}

		if (m_fDirectionRandomTime >= 2.5f) {
			m_fDirectionRandomTime = 0;
			m_bDirection = Random.Range (0, 100) % 2 == 0;
		}
		
		transform.Translate ((m_bDirection?m_fSpeed:-m_fSpeed) * Time.deltaTime, 0, 0);

		if (transform.localPosition.x <= 9f) {
			transform.localPosition = new Vector3 (9f, transform.localPosition.y, 0f);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "wall") {
			m_bDirection = !m_bDirection;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Weapon") 
		{
			coll.gameObject.SendMessage ("ApplyDamage", 10);
		}
	}

}
