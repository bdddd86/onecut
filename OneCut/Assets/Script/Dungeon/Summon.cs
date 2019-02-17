using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour {

	private bool isEnter = false;

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag != "Player")
			return; 

		if (isEnter)
			return;
		
		Debug.Log ("Summon Monster: " + coll.gameObject.name);
		isEnter = true;

		Animator anim = this.GetComponent<Animator> ();
		anim.Rebind ();
		anim.Play ("Summon");

		MonsterSummonManager.instance.SummonsMonster();
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag != "Player")
			return; 

		isEnter = false;
	}
}
