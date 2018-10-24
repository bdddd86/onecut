using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBall : MonoBehaviour {

	public Slider mSlider;
	public Text mPoint;
	public GameObject mMainPop;
	public GameObject mTutorialPop;
	public Button mStartButton;
	public RankingService mRanking;
	public AdMobManager mAdmob;
	[Header("Result")]
	public GameObject mResultPop;
	public Text mResultPoint;
	public Slider mResultSlider;
	[Header("Sound")]
	public AudioSource mAudioBounce;
	public AudioSource mAudioPoint;
	public AudioSource mAudioEnding;
	public ParticleSystem mSmoke;
	[Header("Class")]
	public WallPool mWallPool;

	float mfFreeTime = 0f;

	Vector3 firstTouchPos = Vector3.zero;

	float mfTouchedTime = 0f;

	bool mbInput = false;

	int nScore = 0;
	float fPower = 0f;
	int nDead = 0;

	// Use this for initialization
	void Start () {
		mSlider.minValue = 0f;
		mSlider.maxValue = 30f;

		nDead = PlayerPrefs.GetInt ("GrrPuding_BounceBall_Die", 0);

		StartCoroutine (OnStart ());
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

				fPower = pushedTime * 100f;
				if (fPower <= 3f){
					fPower = 3f;
				}
				if (fPower >= 30f){
					fPower = 30f;
				}
				GetComponent<Rigidbody>().velocity = Vector3.zero;
				GetComponent<Rigidbody>().AddForce(0,-fPower,0,ForceMode.Impulse);

				mSlider.value = fPower;

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
					fPower = distance * 0.1f;
					if (fPower <= 3f){
						fPower = 3f;
					}
					if (fPower >= 30f){
						fPower = 30f;
					}
					GetComponent<Rigidbody>().velocity = Vector3.zero;
					GetComponent<Rigidbody>().AddForce(0,-fPower,0,ForceMode.Impulse);

					mSlider.value = fPower;
				}
				firstTouchPos = Vector3.zero;
			}
		}
		//Debug.Log("Velocity: "+GetComponent<Rigidbody>().velocity.ToString());
		#else
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {
				if (firstTouchPos == Vector3.zero)
				{
					firstTouchPos = Input.mousePosition;
				}
			}
			else if (touch.phase == TouchPhase.Ended) {
				if (firstTouchPos != Vector3.zero)
				{
					float distance = Vector3.Distance(firstTouchPos, Input.mousePosition);
					if (distance > 10f)
					{
						fPower = distance * 0.1f;
						if (fPower <= 3f){
							fPower = 3f;
						}
						if (fPower >= 30f){
							fPower = 30f;
						}
						GetComponent<Rigidbody>().velocity = Vector3.zero;
						GetComponent<Rigidbody>().AddForce(0,-fPower,0,ForceMode.Impulse);

						mSlider.value = fPower;
					}
					firstTouchPos = Vector3.zero;
				}
			}
		}
		#endif

		transform.Rotate (Vector3.back, 5f);
	}

	IEnumerator OnStart()
	{
		mRanking.SignIn ();
		yield return new WaitForSeconds (0.5f);
		mStartButton.enabled = true;
	}

	void OnCollisionEnter(Collision col) {
		if (mbInput == false) {
			// 볼 소리는 넣자.
			if (col.gameObject.CompareTag ("Earth")) {
				mAudioBounce.Play ();
			}
			return;
		}
		
		if (col.gameObject.CompareTag ("Wall")) {
			mAudioEnding.Play ();

			mbInput = false;
			GetComponent<Rigidbody> ().AddExplosionForce (100f, col.transform.position, 360f);
			mWallPool.Stop ();

			mResultPoint.text = string.Format ("<size=70>{0}</size>\nBest <color=#ff0000>{1}</color>",nScore,nScore);
			mResultSlider.value = nScore >= 10 ? 1f : (nScore/10f) + 0.05f;
			mPoint.text = string.Empty;
			mResultPop.SetActive (true);

			nDead += 1;
			if (nDead >= 3) {
				nDead = 0;
				mAdmob.ShowInterstitialAd ();	// 전면광고
			}
			PlayerPrefs.SetInt ("GrrPuding_BounceBall_Die", nDead);
		}
		else if (col.gameObject.CompareTag ("Earth")){
			mAudioBounce.Play();

			if (fPower >= 25f) {
				mSmoke.transform.localPosition = transform.localPosition + Vector3.down;
				mSmoke.Play ();
			}
			fPower = 0f;
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
		mMainPop.SetActive (false);
		mTutorialPop.SetActive (false);

		nScore = 0;
		mPoint.text = nScore.ToString ();
		
		mResultPop.SetActive (false);

		GetComponent<Rigidbody>().velocity = Vector3.zero;
		transform.localPosition = new Vector3(0f, 10f, 0f);
		mbInput = true;
		mWallPool.ReStart ();
	}

	public void ShowTutorial()
	{
		mMainPop.SetActive (false);
		mResultPop.SetActive (false);
		mTutorialPop.SetActive (true);
	}

	bool IsFreeTime()
	{
		if (mfFreeTime == 0f)
			return false;
		
		float fTime = Time.time - mfFreeTime;
		if (fTime < 600f) {
			return true;
		}

		mfFreeTime = 0f;

		return false;
	}
}
