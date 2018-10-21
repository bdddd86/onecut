using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBall : MonoBehaviour {

	public Slider mSlider;

	Vector3 firstTouchPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		mSlider.minValue = 0f;
		mSlider.maxValue = 30f;
	}
	
	// Update is called once per frame
	void Update () {

		#if UNITY_EDITOR
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
}
