using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPool : MonoBehaviour {

	List<GameObject> mListWall = new List<GameObject>();
	float mfDelay = 0f;

	public bool mbStop = false;

	// Use this for initialization
	void Start () {

		for (int i = 0; i < transform.childCount; i++) {
			mListWall.Add (transform.GetChild (i).gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (mbStop)
			return;

		if (mListWall != null && mListWall.Count > 0) {
			mfDelay += Time.deltaTime;

			if (mfDelay >= 3f) {
				mfDelay = 0f;

				for (int i = 0; i < mListWall.Count; i++) {
					if (mListWall [i].activeSelf == false) {
						mListWall [i].transform.localPosition = new Vector3 (12f, -Random.Range(6,24) * 0.5f, 0f);
						mListWall [i].SetActive (true);
						break;
					}
				}
			}

			for (int i = 0; i < mListWall.Count; i++) {
				if (mListWall [i].activeSelf == false)
					continue;
				mListWall [i].transform.Translate (Vector3.left * Time.deltaTime * 5f);

				if (mListWall [i].transform.localPosition.x <= -30f) {
					mListWall [i].SetActive (false);
					//mListWall [i].transform.localPosition = new Vector3 (15f, 0f, 0f);
				}
			}
		}
	}

	public void Stop()
	{
		mbStop = true;
	}

	public void ReStart()
	{
		mfDelay = 0f;

		for (int i = 0; i < mListWall.Count; i++) {
			mListWall [i].transform.localPosition = new Vector3 (12f, -Random.Range(0,25) * 0.5f, 0f);
			mListWall [i].SetActive (false);
		}

		mbStop = false;
	}
}
