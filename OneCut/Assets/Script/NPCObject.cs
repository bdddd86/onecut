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
        }
    }

    private void Update()
    {
        if (lblName)
        {
            // out 처리 필요
            lblName.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, this.gameObject.transform.position);
        }
    }
}
