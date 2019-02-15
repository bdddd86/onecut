using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
	
	public float speed = 5f;
	public float lifeTime = 3000f;
	private float startTime;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag != "Player")
			return; 

		Debug.Log ("Fire Hit: " + coll.gameObject.name);
		this.gameObject.SetActive (false);
	}

	void OnEnable()
	{
		startTime = Time.time;
	}

	void Update () {
		if (gameObject.activeInHierarchy == true) 
		{
			transform.Translate (Vector3.down * Time.deltaTime * speed);

			if (Time.time - startTime >= lifeTime) {
				this.gameObject.SetActive (false);
			}
		}
	}
}
