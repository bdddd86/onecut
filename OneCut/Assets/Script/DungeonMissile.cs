using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMissile : MonoBehaviour {

	//public Vector3 startPos;
	public Vector3 endPos;
	public int dir_right;	// 0:사용안함, 1:오른쪽, 2:왼쪽
	public int dir_top;		// 0:사용안함, 1:위쪽, 2:아래쪽
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
		if (dir_right == 0) {
			if (dir_top == 1) {
				// 위
				this.GetComponent<SpriteRenderer> ().flipX = true;
				direction = Vector3.up;
			} else if (dir_top == 2) {
				// 아래쪽
				this.GetComponent<SpriteRenderer> ().flipX = false;
				direction = Vector3.down;
			} else {
				// 들어오면 안됨.(예외처리로 오른쪽)
				this.GetComponent<SpriteRenderer> ().flipX = false;
				direction = Vector3.right;
			}
		} else if (dir_right == 1) {
			if (dir_top == 0) {
				// 오른쪽
				this.GetComponent<SpriteRenderer> ().flipX = false;
				direction = Vector3.right;
			} else if (dir_top == 1) {
				// 위오른쪽
				this.GetComponent<SpriteRenderer> ().flipX = false;
				direction = Vector3.up + Vector3.right;
			} else if (dir_top == 2) {
				// 아래오른쪽
				this.GetComponent<SpriteRenderer> ().flipX = false;
				direction = Vector3.down + Vector3.right;
			} else {
				// 들어오면 안됨.(예외처리로 왼쪽)
				this.GetComponent<SpriteRenderer> ().flipX = true;
				direction = Vector3.left;
			}
		} else if (dir_right == 2) {
			if (dir_top == 0) {
				// 왼쪽
				this.GetComponent<SpriteRenderer> ().flipX = true;
				direction = Vector3.left;
			} else if (dir_top == 1) {
				// 위왼쪽
				this.GetComponent<SpriteRenderer> ().flipX = true;
				direction = Vector3.up + Vector3.left;
			} else if (dir_top == 2) {
				// 아래왼쪽
				this.GetComponent<SpriteRenderer> ().flipX = true;
				direction = Vector3.down + Vector3.left;
			} else {
				// 들어오면 안됨.(예외처리로 오른쪽)
				this.GetComponent<SpriteRenderer> ().flipX = false;
				direction = Vector3.right;
			}
		}
		//if (startPos.x > endPos.x) {
		//	this.GetComponent<SpriteRenderer> ().flipX = true;
		//}
		//else {
		//	this.GetComponent<SpriteRenderer> ().flipX = false;
		//}

		//transform.position = startPos;
		//direction = endPos - startPos;

		// 캐릭터 위치 기준으로 미사일 시작점 잡음.
		Vector3 charPos = GameManager.instance.character.transform.position;
		transform.position = charPos - (direction * 100f);
		endPos = charPos + (direction * 100f);
	}

	void Update()
	{
		if (gameObject.activeInHierarchy == true) 
		{
			transform.Translate (direction * Time.deltaTime * speed);

			if (dir_right == 0) {
				if (dir_top == 1) {
					// 위
					if (transform.position.y >= endPos.y) {
						gameObject.SetActive (false);
					}
				} else if (dir_top == 2) {
					// 아래쪽
					if (transform.position.y <= endPos.y) {
						gameObject.SetActive (false);
					}
				} else {
					// 들어오면 안됨.(예외처리로 오른쪽)
					if (transform.position.x >= endPos.x) {
						gameObject.SetActive (false);
					}
				}
			} else if (dir_right == 1) {
				if (dir_top == 0) {
					// 오른쪽
					if (transform.position.x >= endPos.x) {
						gameObject.SetActive (false);
					}
				} else if (dir_top == 1) {
					// 위오른쪽
					if (transform.position.x >= endPos.x || transform.position.y >= endPos.y) {
						gameObject.SetActive (false);
					}
				} else if (dir_top == 2) {
					// 아래오른쪽
					if (transform.position.x >= endPos.x || transform.position.y <= endPos.y) {
						gameObject.SetActive (false);
					}
				} else {
					// 들어오면 안됨.(예외처리로 왼쪽)
					if (transform.position.x <= endPos.x) {
						gameObject.SetActive (false);
					}
				}
			} else if (dir_right == 2) {
				if (dir_top == 0) {
					// 왼쪽
					if (transform.position.x <= endPos.x) {
						gameObject.SetActive (false);
					}
				} else if (dir_top == 1) {
					// 위왼쪽
					if (transform.position.x <= endPos.x || transform.position.y >= endPos.y) {
						gameObject.SetActive (false);
					}
				} else if (dir_top == 2) {
					// 아래왼쪽
					if (transform.position.x <= endPos.x || transform.position.y <= endPos.y) {
						gameObject.SetActive (false);
					}
				} else {
					// 들어오면 안됨.(예외처리로 오른쪽)
					if (transform.position.x >= endPos.x) {
						gameObject.SetActive (false);
					}
				}
			}
		}
	}
}
