using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour {
	
	public float speed = 5f;
	public float lifeTime = 5f;
	private float startTime;
	private Vector3 direction;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag != "Player")
			return; 

		Debug.Log ("Dart Hit: " + coll.gameObject.name);
		transform.localPosition = new Vector3 (100f, 100f, 0f);
		this.gameObject.SetActive (false);
	}

	void OnEnable()
	{
		direction = Vector3.zero;

		startTime = Time.time;

		Vector3 charPos = GameManager.instance.character.transform.position;
		//Vector3 startPos = charPos + ((Random.Range(0f, 1f) >= 0.5f)?Vector3.right:Vector3.left * 10f);
		//startPos += Vector3.up * Random.Range (0, 10);

		float circleX = 10f * Mathf.Cos (Random.Range (0f, 359f));
		float circleY = 10f * Mathf.Sin (Random.Range (0f, 359f));
		Vector3 startPos = new Vector3 (circleX + charPos.x, circleY + charPos.y, 0f);

		transform.position = startPos;

		Vector3 dot = charPos - startPos;
		direction = dot.normalized;
	}

	void Update () {
		if (gameObject.activeSelf == true) 
		{
			if (direction != Vector3.zero) {
				transform.Translate (direction * Time.deltaTime * speed);
			}
			if (Time.time - startTime >= lifeTime) {
				this.gameObject.SetActive (false);
			}
		}
	}
}
