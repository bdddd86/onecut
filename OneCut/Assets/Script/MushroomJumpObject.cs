using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomJumpObject : Gimmick {

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 20f), ForceMode2D.Impulse);
        }
    }
}
