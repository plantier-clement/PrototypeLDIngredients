using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseAim : MonoBehaviour {

	public GameObject target;
	public float rotateSpeedX = 5f;
	public float rotateSpeedY = 5f;
	Vector3 rotationOffset;

	public Vector3 offset;

	void Start() {
		rotationOffset = target.transform.position - transform.position;
	}


	void LateUpdate() {

			float horizontal = Input.GetAxis ("Mouse X") * rotateSpeedX;
			float vertical = Input.GetAxis ("Mouse Y") * rotateSpeedY;
			target.transform.Rotate (-vertical, horizontal, 0);

			float desiredAngleX = target.transform.eulerAngles.x;
			float desiredAngleY = target.transform.eulerAngles.y;
			Quaternion rotation = Quaternion.Euler (desiredAngleX, desiredAngleY, 0);
			transform.position = target.transform.position + offset - (rotation * rotationOffset);

			transform.LookAt (target.transform);
	}
}