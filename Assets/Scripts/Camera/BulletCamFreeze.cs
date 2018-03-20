using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCamFreeze : MonoBehaviour {

	Quaternion initRot;
	Camera cam;



	void Start () {
		initRot = transform.rotation;
		cam = GetComponent<Camera> ();
	}
	

	void LateUpdate () {
		cam.transform.rotation = initRot; // freeze camera rotation (bullet can still roll)
	}
}
