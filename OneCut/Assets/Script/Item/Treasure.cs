using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : ItemObject 
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (GameManager.instance.UseItem("key"))
                Destroy(this.gameObject);
        }
    }
}
