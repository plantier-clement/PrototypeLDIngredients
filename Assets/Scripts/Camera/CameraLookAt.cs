using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour {

	public GameObject target;

	public Vector3 offset;

	void LateUpdate()
	{
		Vector3 desiredPosition = target.transform.position + offset;
		transform.LookAt (desiredPosition);
	}


}
