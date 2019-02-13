using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunBam : GimmickObject {

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Collision");
            GameManager.instance.RestartGame();
        }
    }
}
