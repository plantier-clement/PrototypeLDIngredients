using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour {


	[SerializeField]
	float clampMinAngle = -45.0f;

	[SerializeField]
	float clampMaxAngle = 45.0f;

	public void SetRotation (float amount){

		float clampedAngle = GetClampedAngle (amount);
		transform.eulerAngles = new Vector3 (clampedAngle, transform.eulerAngles.y, transform.eulerAngles.z);
	}

	private float GetClampedAngle(float amount){
	
		float newAngle = CheckAngle (transform.eulerAngles.x - amount);
		float clampedAngle = Mathf.Clamp (newAngle, clampMinAngle, clampMaxAngle);
		return clampedAngle;
	}


	public float GetAngle(){
		return CheckAngle (transform.eulerAngles.x);
	}

	public float CheckAngle(float value){
		float angle = value - 180;

		if (angle > 0)
			return angle - 180;

		return angle + 180;
	}



}
