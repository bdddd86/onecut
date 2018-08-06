using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMissile : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;
	public float speed;
	private Vector3 direction;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag != "Player")
			return;

		Debug.Log ("Hit: " + coll.gameObject.name);
		this.gameObject.SetActive (false);

		//sparkle.transform.position = this.transform.position;
		//sparkle.SetTrigger ("sparkle");
	}

	void OnEnable()
	{
		if (startPos.x > endPos.x) {
			this.GetComponent<SpriteRenderer> ().flipX = true;
		}
		else {
			this.GetComponent<SpriteRenderer> ().flipX = false;
		}

		transform.position = startPos;
		direction = endPos - startPos;
	}

	void Update()
	{
		if (gameObject.activeInHierarchy == true) 
		{
			transform.Translate (direction * Time.deltaTime * speed);

			// 우측에서 좌측으로
			if (startPos.x > endPos.x) {
				// 위에서 아래로
				if (startPos.y > endPos.y) {
					if (transform.position.x <= endPos.x && transform.position.y <= endPos.y) {
						// 끝
						this.gameObject.SetActive (false);
					}
				}
				else {
					// 아래에서 위로
					if (transform.position.x <= endPos.x && transform.position.y >= endPos.y) {
						// 끝
						this.gameObject.SetActive (false);
					}
				}
			} 
			else {
				// 좌측에서 우측으로
				// 위에서 아래로
				if (startPos.y > endPos.y) {
					if (transform.position.x >= endPos.x && transform.position.y <= endPos.y) {
						// 끝
						this.gameObject.SetActive (false);
					}
				}
				else {
					// 아래에서 위로
					if (transform.position.x >= endPos.x && transform.position.y >= endPos.y) {
						// 끝
						this.gameObject.SetActive (false);
					}
				}
			}
		}
	}
}
