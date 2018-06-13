using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour {
    public string key;
    Text lbl;

    private void Start()
    {
        lbl = GetComponent<Text>();
        lbl.text = UtillFunc.Instance.GetLocalizedText(key);
    }
}
