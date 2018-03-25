using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour {

	public float cameraMoveSpeed = 120.0f;
	Vector3 followPos;

	public GameObject CameraFollowObj;
	public GameObject CameraObj;
	public GameObject PlayerObj;

	public float clampAngle = 80.0f;
	public float inputSensitivity = 150.0f;
	float camDistanceXToPlayer;
	float camDistanceYToPlayer;
	float camDistanceZToPlayer;

	float mouseX;
	float mouseY;
	float finalInputX;
	float finalInputZ;

	public float smoothX;
	public float smoothY;
	float rotationX = 0.0f;
	float rotationY = 0.0f;


	void Start(){
		Vector3 rot = transform.localRotation.eulerAngles;
		rotationX = rot.x;
		rotationY = rot.y;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}


	void Update () 
	{
		// setup rotations of the stiks
		float inputX = Input.GetAxis("RightStickHorizontal");
		float inputZ = Input.GetAxis("RightStickVertical");
		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");

		finalInputX = inputX + mouseX;
		finalInputZ = inputZ + mouseY;

		rotationY += finalInputX * inputSensitivity * Time.deltaTime;
		rotationX += finalInputZ * inputSensitivity * Time.deltaTime;

		rotationX = Mathf.Clamp (rotationX, -clampAngle, clampAngle);
		Quaternion localRotation = Quaternion.Euler (rotationX, rotationY, 0.0f);
		transform.rotation = localRotation;

	}

	void LateUpdate (){
		CameraUpdater ();
	}


	void CameraUpdater(){
		// set target object to follow
		Transform target = CameraFollowObj.transform;
		//move towards 

		float step = cameraMoveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.position, step);

	}

}
