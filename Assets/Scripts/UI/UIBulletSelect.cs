using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBulletSelect : MonoBehaviour {

	public Image redImage;
	public Image greenImage;
	public Image blueImage;
	public Image activeBulletImg;

	public GameObject bulletCamera;
	Vector3 bulletCameraInitScale;
	public Vector3 bulletCameraFocusScale;
	public float scaleSpeed = 1f;

	void Start () {
		ResetUI ();
	}


	public void UpdateUI (int bulletID){
		if (bulletID == 0) {
			redImage.color = Color.red;
		} else if (bulletID == 1) {
			greenImage.color = Color.green;
		} else {
			blueImage.color = Color.blue;
		}
	}

	public void SwitchActiveBullet (List<GameObject> bulletList, GameObject activeBullet, int bulletID)	{
		// UI feedback: display & set position
		activeBulletImg.enabled = true;
		switch (bulletID) {
		case 0:
			activeBulletImg.rectTransform.anchoredPosition3D = new Vector3 (10f, 25, 0);
			break;
		case 1:
			activeBulletImg.rectTransform.anchoredPosition3D = new Vector3 (-50f, 25, 0);
			break;
		case 2: 
			activeBulletImg.rectTransform.anchoredPosition3D = new Vector3 (-110f, 25, 0);
			break;
		}

		// disable all bullet cameras before enabling the active one
		for (int i = 0; i < bulletList.Count; i++) {
			Camera cam = bulletList[i].GetComponent<Camera> ();
			cam.enabled = false;
		}
		Camera activeCam = activeBullet.GetComponent<Camera> ();
		activeCam.enabled = true;
		bulletCamera.SetActive (true);
	}

	public void ResetUI() {
		redImage.color = Color.grey;
		greenImage.color = Color.grey;
		blueImage.color = Color.grey;
		activeBulletImg.enabled = false;
		bulletCamera.SetActive (false);
		bulletCameraInitScale = bulletCamera.transform.localScale;
	}

	public void FocusBullet(){
		StartCoroutine (ScaleUp());
		// mouse axis X : rotate active bullet cam && don't rotate main camera
		}
		
	public void UnfocusBullet(){
		StartCoroutine (ScaleDown());
	}

	public IEnumerator ScaleUp() {
		while (bulletCameraFocusScale.x > bulletCamera.transform.localScale.x) {
			bulletCamera.transform.localScale += new Vector3 (1, 1, 1) * Time.deltaTime * scaleSpeed;
			yield return null;
		}
		StopCoroutine (ScaleUp ());
	}

	public IEnumerator ScaleDown() {
		while (bulletCamera.transform.localScale.x > bulletCameraInitScale.x) {
			bulletCamera.transform.localScale -= new Vector3 (1, 1, 1) * Time.deltaTime * scaleSpeed;
			yield return null;
		}
		StopCoroutine (ScaleDown ());
	}


}