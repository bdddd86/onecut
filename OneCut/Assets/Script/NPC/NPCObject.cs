using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class NpcObject : MonoBehaviour 
{
    public Text lblName;
    public string npcName;

    private void Start()
    {
        if (lblName != null)
        {
            lblName.text = npcName;
        }
    }

    private void Update()
    {
        if (lblName != null)
        {
            // out 처리 필요
			lblName.transform.position = this.transform.position + (Vector3.up * 0.5f);//RectTransformUtility.WorldToScreenPoint(Camera.main, this.gameObject.transform.position);
        }
    }
}
