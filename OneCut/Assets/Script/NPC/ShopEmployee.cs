using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEmployee : NpcObject 
{
    private void OnMouseDown()
    {
        GameManager.instance.OpenShop();
    }
}
