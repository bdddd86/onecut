using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 10 Point 넘기면 성공송. 폭죽 이팩트.
// 하단에 목표설정 만들어주기.(광고자리 피해서)
// 0점 ???(웃음소리)
// 1점 답답해요
// 2-3점 잘좀해봐요
// 4-5점 평균이상이군
// 6-7점 좀 잘하는걸?
// 8-9점 놀라운 점수
// 10점 신

public class InputBall : MonoBehaviour {

	public Slider mSlider;
	public Text mPoint;
	[Header("Result")]
	public GameObject mResultPop;
	public Text mResultPoint;
	public Text mBestPoint;
	[Header("Sound")]
	public AudioSource mAudioBounce;
	public AudioSource mAudioPoint;
	public AudioSource mAudioEnding;
	[Header("Class")]
	public WallPool mWallPool;

	Vector3 firstTouchPos = Vector3.zero;

	float mfTouchedTime = 0f;

	bool mbInput = true;

	int nScore = 0;

	// Use this for initialization
	void Start () {
		mSlider.minValue = 0f;
		mSlider.maxValue = 30f;
	}

	// Update is called once per frame
	void Update () {

		if (mbInput == false)
			return;
		
		#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
		if (Input.GetKeyDown (KeyCode.Space)) {
			//Debug.Log ("Space Down");
			if (mfTouchedTime == 0f) {
				mfTouchedTime = Time.time;
			}
		} else if (Input.GetKeyUp (KeyCode.Space)) {
			//Debug.Log ("Space Key");
			if (mfTouchedTime != 0f) {
				float pushedTime = Time.time - mfTouchedTime;

				float power = pushedTime * 100f;
				if (power <= 3f){
					power = 3f;
				}
				if (power >= 30f){
					power = 30f;
				}
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				GetComponent<Rigidbody>().AddForce(0,-power,0,ForceMode.Impulse);

				mSlider.value = power;

				mfTouchedTime = 0f;
			}
		}

		if (Input.GetMouseButton (0)) {
			//Debug.Log("GetMouseButton");
			if (firstTouchPos == Vector3.zero)
			{
				//Debug.Log("입력");
				firstTouchPos = Input.mousePosition;
			}
		}
		else if (Input.GetMouseButtonDown (0)) {
			//Debug.Log("GetMouseButtonDown");
			// 여기안들어옴.
		}
		else if (Input.GetMouseButtonUp (0)) {
			//Debug.Log("GetMouseButtonUp");
			if (firstTouchPos != Vector3.zero)
			{
				float distance = Vector3.Distance(firstTouchPos, Input.mousePosition);
				//Debug.Log("Distance: "+distance.ToString());
				if (distance > 10f)
				{
					float power = distance * 0.1f;
					if (power <= 3f){
						power = 3f;
					}
					if (power >= 30f){
						power = 30f;
					}
					GetComponent<Rigidbody>().velocity = Vector3.zero;
					GetComponent<Rigidbody>().AddForce(0,-power,0,ForceMode.Impulse);

					mSlider.value = power;
				}
				firstTouchPos = Vector3.zero;
			}
		}
		//Debug.Log("Velocity: "+GetComponent<Rigidbody>().velocity.ToString());
		#else
		#endif

		transform.Rotate (Vector3.back, 5f);
	}

	void OnCollisionEnter(Collision col) {
		if (mbInput == false)
			return;
		
		if (col.gameObject.CompareTag ("Wall")) {
			mAudioEnding.Play ();

			mbInput = false;
			GetComponent<Rigidbody> ().AddExplosionForce (100f, col.transform.position, 360f);
			mWallPool.Stop ();

			mResultPop.SetActive (true);
		}
		else if (col.gameObject.CompareTag ("Earth")){
			mAudioBounce.Play();
		}
	}

	void OnTriggerEnter(Collider col){
		if (mbInput == false)
			return;
		
		if (col.CompareTag ("Score")) {
			mAudioPoint.Play ();
			nScore += 1;
			mPoint.text = nScore.ToString ();
		}
	}

	public void ReStart()
	{
		nScore = 0;
		mPoint.text = nScore.ToString ();
		
		mResultPop.SetActive (false);

		GetComponent<Rigidbody>().velocity = Vector3.zero;
		transform.localPosition = new Vector3(0f, 10f, 0f);
		mbInput = true;
		mWallPool.ReStart ();
	}
}
