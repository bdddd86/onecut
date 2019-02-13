using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
	private Animator m_anim;
	public List<SpriteRenderer> m_listSprite; 

	void Start () 
	{
		m_anim = GetComponent<Animator> ();
	}

	public void PlayShot(bool bRight = true)
	{
		this.transform.position = GameManager.instance.character.transform.position + ((bRight?Vector3.right:Vector3.left) * 1.5f);
		for (int i = 0; i < m_listSprite.Count; i++) 
		{
			m_listSprite [i].flipX = !bRight;
		}
		m_anim.ResetTrigger ("shot");
		m_anim.SetTrigger ("shot");
	}

	// 애니메이션에서 호출하는 타격점.
	public void CallLazing()
	{
		MonsterSummonManager.instance.OnRecvGlobalAttack (100);
	}
	// 막타에서 호출.
	public void CallLazing_Last()
	{
		MonsterSummonManager.instance.OnRecvGlobalAttack (200);
	}

	// 테스트용
	#if UNITY_EDITOR
	void OnGUI()
	{
		if (GUI.Button (new Rect (10, 10, 50, 50), "Lazer")) {
			//PlayShot (GameManager.instance.character.IsRight());
			GameManager.instance.character.LazerAttack();
		}
		if (GUI.Button (new Rect (10, 60, 50, 50), "Damage")) {
			GameManager.instance.character.Damage(30);
		}
		if (GUI.Button (new Rect (10, 110, 50, 50), "Dungeon Attack")) {
			DungeonManager.instance.Attack ();
		}
	}
	#endif
}
