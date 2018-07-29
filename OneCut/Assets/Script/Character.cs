using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Character : MonoBehaviour
{
    public Rigidbody2D rigidBody;
	public Animator animator;
	public SpriteRenderer spriteRenderer;
	public List<GameObject> listBullet;
	public Bomb bomb;
	public Animator fxDust;
	public Transform imgLife;
	public Transform imgSkillLife;

    public System.Action<string> useItemAction;
    public Dictionary<string, int> inventory;   

	public Lazer lazer;
	public Character3DEffectManager effect3DManager;

	private int m_nFrameCnt = 0;
	private bool m_bEvasion = false;

    void Start()
    {
		animator.SetFloat ("velocity", 0);
    }

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
		if (Input.GetKeyDown(KeyCode.F))
		{
			Evasion();
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
            //Debug.Log(string.Format("H:{0} V:{1}", fHorizontal, fVertical));
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
                //Debug.Log(string.Format("V: {0}", rigidBody.velocity));
                if (Mathf.Abs(rigidBody.velocity.x) <= 0.1f)
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }
        }

		// 속도 체크
		animator.SetFloat ("velocity", Mathf.Abs (rigidBody.velocity.x));

		// 방향 체크
		if (rigidBody.velocity.x < 0 && spriteRenderer.flipX == false) {
			spriteRenderer.flipX = true;
			// 먼지 이팩트
			fxDust.transform.localPosition = new Vector3 (0.7f, 0.2f, 0);
			fxDust.GetComponent<SpriteRenderer> ().flipX = true;
			fxDust.SetTrigger ("dust");
		} else if (rigidBody.velocity.x > 0 && spriteRenderer.flipX == true) {
			spriteRenderer.flipX = false;
			// 먼지 이팩트
			fxDust.transform.localPosition = new Vector3 (-0.7f, 0.2f, 0);
			fxDust.GetComponent<SpriteRenderer> ().flipX = false;
			fxDust.SetTrigger ("dust");
		}

        // 카메라 따라가기.
        if (transform.localPosition.x != Camera.main.transform.localPosition.x)
        {
            Camera.main.transform.localPosition = new Vector3(transform.localPosition.x, 0f, -8f);
        }

		// UI 업데이트. 2프레임당 1회.
		if (m_nFrameCnt != Time.frameCount) 
		{
			m_nFrameCnt = Time.frameCount;
			if (m_nFrameCnt % 2 == 0) 
			{
				UpdateUI ();
			}
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
		GameManager.instance.attackCount += 1;
		if (GameManager.instance.attackCount > 10) {
			animator.SetTrigger ("bomb");
		} 
		else {
			animator.SetTrigger ("attack");
		}
		// 애니메이션 중 Shot 호출.
    }

	public void LazerAttack()
	{
		animator.SetTrigger ("sp_attack");
		// 애니메이션에서 트리거로 GoLazerAttack 호출.
	}
	public void GoLazerAttack()
	{
		lazer.PlayShot (IsRight());
	}

	// 회피
	void Evasion()
	{
		m_bEvasion = true;

		// HP 감춘다. 어차피 무적판정중.
		imgLife.gameObject.SetActive(false);
		imgSkillLife.gameObject.SetActive (false);

		fxDust.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0f); 

		if (rigidBody.velocity.x == 0)
			rigidBody.AddForce(new Vector2(IsRight()?500f:-500f, 100f));
		
		animator.SetTrigger (IsRight()?"evasion":"evasion_left");
		// 애니메이션에서 EndEvasion 호출함.
	}
	public void EndEvasion()
	{
		m_bEvasion = false;

		// HP 등장. 무적끝.
		imgLife.gameObject.SetActive(true);
		imgSkillLife.gameObject.SetActive (true);

		fxDust.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
	}

	// 피해
	public void Damage(int damage)
	{
		GameManager.instance.HitPoints -= damage;
		if (GameManager.instance.HitPoints <= 0) {
			Die ();
		} 
		else {
			animator.SetTrigger ("damage");
		}
	}

	// 광역피해 전달. (원점, 범위, 데미지)
	public void OnRecvAreaAttack(Vector3 org, float distance, int damage)
	{
		if (distance >= Vector3.Distance (this.transform.localPosition, org)) {
			Damage (damage);
		}
	}

	// 죽음
	public void Die()
	{
	}

	public bool IsRight()
	{
		return spriteRenderer.flipX == false;
	}

	public void Shot()
	{
		if (GameManager.instance.attackCount > 10) {
			GameManager.instance.attackCount = 0;
			// 폭탄공격
			bomb.Shot (this.transform.localPosition, 
				this.transform.localPosition + (Vector3.up * 5f) + (spriteRenderer.flipX ? (Vector3.left * 1.2f) : (Vector3.right * 1.2f)),
				this.transform.localPosition + (Vector3.up * 0.55f) + (spriteRenderer.flipX ? (Vector3.left * 3f) : (Vector3.right * 3f)));
		} 
		else {
			// 총알쏘기
			GameObject bullet = null;
			for (int i = 0; i < listBullet.Count; i++) {
				if (listBullet [i].activeInHierarchy == false) {
					bullet = listBullet [i];
					break;
				}
			}

			if (bullet == null) {
				Debug.Log ("총알없음");
				return;
			}

			if (spriteRenderer.flipX == true) {
				// 좌측
				bullet.GetComponent<Bullet>().direction = Vector3.left;
				bullet.transform.localPosition = this.transform.localPosition + Vector3.left;
			} 
			else {
				// 우측
				bullet.GetComponent<Bullet>().direction = Vector3.right;
				bullet.transform.localPosition = this.transform.localPosition + Vector3.right;
			}
			bullet.SetActive (true);
		}
	}

	public void Exp(object level)
	{
		int getExp = UtillFunc.Instance.GetMonsterExp ((int)level);
		GameManager.instance.totalEXP += getExp;
		Debug.Log (string.Format("#획득 경험치:{0} 총경험치:{1}",getExp,GameManager.instance.totalEXP));

		// 경험치로 레벨업 계산하기.
		int nNextLevelExp = UtillFunc.Instance.GetTotalExpFromLv(GameManager.instance.Level + 1);
		if (GameManager.instance.totalEXP >= nNextLevelExp) {
			// 레벨업.
			GameManager.instance.SettingCharacterInfo (GameManager.instance.Level + 1);
			effect3DManager.PlayEffect (eEffect.eLevelUp);
		}
		GameManager.instance.SettingGameInfoText ();
	}

	public void UpdateUI()
	{
		// 생명력 업데이트
		int maxHitpoint = UtillFunc.Instance.GetHitPoints(GameManager.instance.Level);
		float rateHitpoint = (GameManager.instance.HitPoints*1f) / (maxHitpoint*1f);
		imgLife.localScale = new Vector3 (rateHitpoint * 8f, 1f, 1f);

		// 총알카운트(꽉차면 폭탄공격 한번씩 함)
		float rateAttackCnt = (GameManager.instance.attackCount*1f) / 10f;
		imgSkillLife.localScale = new Vector3 (rateAttackCnt * 8f, 0.5f, 1f);
	}

}
