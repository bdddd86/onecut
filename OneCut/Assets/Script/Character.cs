using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Character : MonoBehaviour
{
    public Rigidbody2D rigidBody;
	public Animator animator;
	public SpriteRenderer spriteRenderer;

    public System.Action<string> useItemAction;
    public Dictionary<string, int> inventory;   

    void Start()
    {
		animator.SetFloat ("velocity", 0);
    }

    // Update is called once per frame
    void Update()
    {
        // 유저 입력.
        float fHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float fVertical = CrossPlatformInputManager.GetAxis("Vertical");

		//if (attackCollider != null && attackCollider.activeInHierarchy == true) {
		//	m_fAttackBoxDelay += (Time.deltaTime * GameManager.instance.m_fAttackSpeed);
		//	m_fAttackDelay = 0;
		//	if (m_fAttackBoxDelay >= 0.05f) {
		//		m_fAttackBoxDelay = 0;
		//		attackCollider.SetActive (false);
		//	}
		//} else {
		//	m_fAttackDelay += (Time.deltaTime * GameManager.instance.m_fAttackSpeed);
		//}

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.W))
		{
			Jump();
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			//Debug.Log("Attack Delay: "+m_fAttackDelay.ToString());
			//if (m_fAttackDelay >= 0.25f) {
			//	m_fAttackDelay = 0;
				Attack();
			//}
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
			//if (m_fAttackDelay >= 0.25f) {
			//	m_fAttackDelay = 0;
				Attack();
			//}
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

		// 속도 체크
		animator.SetFloat ("velocity", Mathf.Abs (rigidBody.velocity.x));

		// 방향 체크
		spriteRenderer.flipX = rigidBody.velocity.x < 0;

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

		animator.SetTrigger ("jump");
    }
    void Attack()
    {
		//if (attackCollider == null)
		//	return;
		
        Debug.Log("Attack");
		//attackCollider.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0);
		//attackCollider.GetComponent<BoxCollider2D> ().size = new Vector2 (2f, 2f);
		//attackCollider.SetActive (true);

		animator.SetTrigger ("attack");
    }

	public void Exp(object level)
	{
		int getExp = UtillFunc.Instance.monsterExp ((int)level);
		Debug.Log ("Get Exp: "+getExp.ToString());
		GameManager.instance.m_nExp += getExp;

		// 테스트 코드
		if (GameManager.instance.m_nExp >= 5) {
			GameManager.instance.m_nExp = 0;
			GameManager.instance.m_nLevel += 1;
			GameManager.instance.SettingCharacterInfo (GameManager.instance.m_nLevel);
			GameManager.instance.SettingGameInfoText ();
		}
	}

    public void AquireItem()
    {
        
    }

    public void UseItem()
    {
        
    }

}
