using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	public Vector3 direction;
	public float speed;
	public float lifeTime;
	public float startTime;
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log ("Hit: " + coll.gameObject.name);
		this.gameObject.SetActive (false);

		// 이팩트, 사운드 추가.
	}

	void OnEnable()
	{
		startTime = Time.time;
		//transform.localPosition = new Vector3 (1f, 0, 0);
		iTween.Stop (this.gameObject);
		transform.localScale = Vector3.one * 0.8f;
		iTween.ScaleTo (this.gameObject, iTween.Hash ("scale", Vector3.one, "time", 0.1f, "islocal", true));
	}

	void Update()
	{
		if (gameObject.activeInHierarchy == true) 
		{
			transform.Translate (direction * Time.deltaTime * speed);


			if (Time.time - startTime >= lifeTime) {
				Debug.Log (string.Format("Distance Over: {0} - {1} >= {2}",Time.time,startTime,lifeTime));
				this.gameObject.SetActive (false);
			}
		}
	}
}
