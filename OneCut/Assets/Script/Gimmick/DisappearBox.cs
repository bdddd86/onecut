using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearBox : GimmickObject 
{
    private float disappearAlphaValue = 0.3f;
    public bool bStartDisappeared; 
    public float waitTime; 
    public iTweenAlphaTo tweenAlpha;
    public BoxCollider2D boxCollider2D;

    private void Awake()
    {
        this.StartCoroutine("ResetToBeginningTween");
    }

    void ResetToBeginning()
    {
        if (bStartDisappeared)
        {
            tweenAlpha.valueFrom = disappearAlphaValue;
            tweenAlpha.valueTo = 1f;
            boxCollider2D.enabled = false;
        }
        else
        {
            tweenAlpha.valueFrom = 1f;
            tweenAlpha.valueTo = disappearAlphaValue;
            boxCollider2D.enabled = true;
        }
        tweenAlpha.ResetToBeginning();
    }

    public void OnCompleteTween()
    {
        if(bStartDisappeared)
        {
            boxCollider2D.enabled = true;
        }
        else
        {
            boxCollider2D.enabled = false;
        }
        this.StartCoroutine("WaitDelayTime");
    }

    IEnumerator WaitDelayTime()
    {
        yield return new WaitForSeconds(waitTime);
        this.StartCoroutine("ResetToBeginningTween");
        yield break; 
    }

    IEnumerator ResetToBeginningTween()
    {
        ResetToBeginning();
        yield return new WaitForSeconds(waitTime);
        tweenAlpha.iTweenPlay();
        yield break;
    }

}
