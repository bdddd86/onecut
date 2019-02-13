using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 오브젝트.
public class ItemObject : MonoBehaviour 
{
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PickupItem();   
        }
    }

    public virtual void PickupItem()
    {
        
    }
}
