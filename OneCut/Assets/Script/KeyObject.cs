using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : ItemObject {
    public SpriteRenderer spriteRenderer; 
    public Sprite notExistSprite;

    public string itemName = "key";

    private bool notExist;
    private void Awake()
    {
        notExist = false; 
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (notExist)
            return; 
        
        base.OnTriggerEnter2D(other);
    }

    public override void PickupItem()
    {
        notExist = true;
        spriteRenderer.sprite = notExistSprite; 
        //GameManager.instance.AquireItem(itemName);
    }
}
