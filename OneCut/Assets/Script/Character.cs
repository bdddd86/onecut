﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Character : MonoBehaviour
{
    public Rigidbody2D rigidBody;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 유저 입력.
        float fHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float fVertical = CrossPlatformInputManager.GetAxis("Vertical");

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.W))
		{
			Jump();
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Attack();
		}
		if (Input.GetKey(KeyCode.A))
		{
			fHorizontal = -1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			fHorizontal = 1;
		}
#endif

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (CrossPlatformInputManager.GetButtonDown("Attack"))
        {
            Attack();
        }

        if (CrossPlatformInputManager.GetButtonDown("Summons"))
        {
            Debug.Log("Summons");
            MonsterSummons.instance.SummonsMonster();
        }

        if (fHorizontal != 0)
        {
            Debug.Log(string.Format("H:{0} V:{1}", fHorizontal, fVertical));
            rigidBody.AddForce(new Vector2(fHorizontal * 50, 0f));
            if (Mathf.Abs(rigidBody.velocity.x) > 10f)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x < 0 ? -10f : 10f, rigidBody.velocity.y);
            }
        }
        else
        {
            if (Mathf.Abs(rigidBody.velocity.x) != 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x / 2, rigidBody.velocity.y);
                Debug.Log(string.Format("V: {0}", rigidBody.velocity));
                if (Mathf.Abs(rigidBody.velocity.x) <= 0.1f)
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }
        }

        // 카메라 따라가기.
        if (transform.localPosition.x != Camera.main.transform.localPosition.x)
        {
            Camera.main.transform.localPosition = new Vector3(transform.localPosition.x, 0f, -8f);
        }
    }

    void Jump()
    {
        Debug.Log("Jump");
        //Debug.Log (string.Format ("A: {0}",rigidBody.angularVelocity));
        if (rigidBody.velocity.y == 0)
            rigidBody.AddForce(new Vector2(0f, 500f));
    }
    void Attack()
    {
        Debug.Log("Attack");
    }

}
