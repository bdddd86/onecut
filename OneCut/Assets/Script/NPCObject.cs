using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class NPCObject : MonoBehaviour 
{
    public Text lblName;
    public string npcName;
    private void Start()
    {
        if (lblName)
        {
            lblName.text = npcName;
            lblName.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, this.gameObject.transform.position);
        }
    }
}
