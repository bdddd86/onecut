using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterData
{
	public int m_nLevel;	// 레벨
	public int m_nPow;		// 힘
	public int m_nInc;		// 지능
	public int m_nDex;		// 민첩

	public float m_fMinAttack;
	public float m_fMaxAttack;
	public float m_fDefence;
	public int m_nLife;
	public float m_fCritical;
	public float m_fBamp;
}

public class Character : MonoBehaviour {

	public Rigidbody2D rigidBody;
	public Camera mainCam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		// 유저 입력.
		float fHorizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
		float fVertical = CrossPlatformInputManager.GetAxis ("Vertical");

		if (CrossPlatformInputManager.GetButtonDown ("Jump")) {
			Debug.Log ("Jump");
			//Debug.Log (string.Format ("A: {0}",rigidBody.angularVelocity));
			if (rigidBody.velocity.y == 0)
				rigidBody.AddForce (new Vector2 (0f, 500f));
		}

		if (CrossPlatformInputManager.GetButtonDown ("Attack")) {
			Debug.Log ("Attack");
		}

		if (CrossPlatformInputManager.GetButtonDown ("Summons")){
			Debug.Log ("Summons");
			MonsterSummons.instance.SummonsMonster();
		}

		if (fHorizontal != 0) 
		{
			Debug.Log (string.Format ("H:{0} V:{1}", fHorizontal, fVertical));
			rigidBody.AddForce (new Vector2 (fHorizontal * 50, 0f));
			if (Mathf.Abs (rigidBody.velocity.x) > 10f) {
				rigidBody.velocity = new Vector2 (rigidBody.velocity.x < 0 ? -10f : 10f, rigidBody.velocity.y);
			}
		}
		else
		{
			if (Mathf.Abs (rigidBody.velocity.x) != 0) {
				rigidBody.velocity = new Vector2 (rigidBody.velocity.x/2, rigidBody.velocity.y);
				Debug.Log (string.Format("V: {0}",rigidBody.velocity));
				if (Mathf.Abs (rigidBody.velocity.x) <= 0.1f) {
					rigidBody.velocity = new Vector2 (0, rigidBody.velocity.y);
				}
			}
		}

		// 카메라 따라가기.
		if (transform.localPosition.x != mainCam.transform.localPosition.x) {
			mainCam.transform.localPosition = new Vector3 (transform.localPosition.x, 0f, -8f);
		}
	}
}
