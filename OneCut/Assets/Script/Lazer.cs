using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour {

	private Animator m_anim;
	public List<SpriteRenderer> m_listSprite; 

	//public SpriteRenderer imgLazer;
	//public float m_fSpeed = 1f;
	//private float m_fTime = 0f;

	// Use this for initialization
	void Start () {
		m_anim = GetComponent<Animator> ();
	}

	public void PlayShot(bool bRight = true)
	{
		this.transform.position = GameManager.instance.character.transform.position + (bRight?Vector3.right:Vector3.left);
		for (int i = 0; i < m_listSprite.Count; i++) {
			m_listSprite [i].flipX = !bRight;
		}
		m_anim.ResetTrigger ("shot");
		m_anim.SetTrigger ("shot");
	}

	// 애니메이션에서 호출하는 타격점.
	public void CallLazing()
	{
		MonsterSummons.instance.OnRecvGlobalAttack (100);
	}
	// 막타에서 호출.
	public void CallLazing_Last()
	{
		MonsterSummons.instance.OnRecvGlobalAttack (200);
	}

	// 테스트용
	#if UNITY_EDITOR
	void OnGUI()
	{
		if (GUI.Button (new Rect (10, 10, 50, 50), "Lazer")) {
			PlayShot (GameManager.instance.character.IsRight());
		}
	}
	#endif
}
