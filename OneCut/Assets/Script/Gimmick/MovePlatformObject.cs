using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformObject : GimmickObject {
    public BoxCollider2D boxCollider2D; 
    public iTweenPositionTo iTweenPosition;

    private Vector3 startPosition;
    private Vector3 endPosition; 

    public void Awake()
    {
        startPosition = iTweenPosition.valueFrom;
        endPosition = iTweenPosition.valueTo;
    }

    void MoveForward()
    {
        iTweenPosition.valueFrom = startPosition;
        iTweenPosition.valueTo = endPosition; 
        iTweenPosition.iTweenPlay();
    }

    void MoveReverse()
    {
        iTweenPosition.valueFrom = this.gameObject.transform.position;
        iTweenPosition.valueTo = startPosition; 
        iTweenPosition.iTweenPlay();
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "foot")
        {
            boxCollider2D.isTrigger = false; 
            MoveForward();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {   
        MoveReverse();
        boxCollider2D.isTrigger = true;
    }
}
