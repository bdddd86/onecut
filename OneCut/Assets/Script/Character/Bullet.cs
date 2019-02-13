using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	public Vector3 direction;
	public float speed;
	public float lifeTime;
	public float startTime;
	public Animator sparkle;

	void OnTriggerEnter2D(Collider2D coll)
	{
        if (coll.gameObject.tag == "npc")
            return; 
        
		Debug.Log ("Hit: " + coll.gameObject.name);
		this.gameObject.SetActive (false);

		sparkle.transform.position = this.transform.position;
		//sparkle.gameObject.SetActive (true);
		sparkle.SetTrigger ("sparkle");
	}

	void OnEnable()
	{
		startTime = Time.time;

		if (direction == Vector3.left) {
			this.GetComponent<SpriteRenderer> ().flipX = true;
		} 
		else {
			this.GetComponent<SpriteRenderer> ().flipX = false;
		}
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
