using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObject : MonoBehaviour 
{
    private void OnMouseDown()
    {
        GameManager.instance.OpenShop();
    }
}
