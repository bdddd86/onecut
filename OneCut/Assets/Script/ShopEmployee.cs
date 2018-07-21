using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEmployee : NPCObject 
{
    private void OnMouseDown()
    {
        GameManager.instance.OpenShop();
    }
}
