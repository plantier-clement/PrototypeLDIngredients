using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportGun : MonoBehaviour {

	public GameObject GunExit;
	public GameObject Bullet;

	public Material[] bulletMaterials;
	public UIBulletSelect UIBulletSelectScript;
	public CameraMouseAim playerCam;

	public float bulletSpeed = 10f;
	public float maxBullet = 3f;

	int currentBulletNumber = 0;
	List<GameObject> bulletList = new List<GameObject>(); 

	GameObject activeBullet;
	MeshRenderer bulletMeshRenderer;


	void start(){
		
	}

	void Shoot(){
		GameObject clone;
		clone = Object.Instantiate(Bullet, GunExit.transform.position, GunExit.transform.rotation); //shoot the bullet
		Rigidbody rb;
		rb = clone.GetComponent<Rigidbody> ();
		rb.velocity = transform.TransformDirection (Vector3.forward * bulletSpeed);		// add force to it

		bulletList.Add (clone);
		bulletMeshRenderer = clone.GetComponent<MeshRenderer> ();

		switch (currentBulletNumber) { // assign material depending on 
		case 0:
			bulletMeshRenderer.material = bulletMaterials[0];
			break;
		
		case 1:
			bulletMeshRenderer.material = bulletMaterials[1];
			break;

		case 2:
			bulletMeshRenderer.material = bulletMaterials[2];
			break;
		}

		currentBulletNumber ++;
		UIBulletSelectScript.UpdateUI(bulletList.IndexOf(clone)); // bullet shot is shown on UI

		if (activeBullet == null) { // if bullet is the first, set it to active bullet 
			activeBullet = clone;
			UIBulletSelectScript.SwitchActiveBullet (bulletList, activeBullet, bulletList.IndexOf(activeBullet)); // UI: feedback active bullet, 
		}
			
	}

	void ClearBullets() { //destroy all bullet & reset list & ui
		for (int i = 0; i < bulletList.Count; i++) {
			Destroy (bulletList [i]);
		}
		currentBulletNumber = 0;
		activeBullet = null;
		bulletList.Clear();
		UIBulletSelectScript.ResetUI ();
	}

	void Teleport() {
		this.gameObject.transform.position = activeBullet.transform.position;
	}
		
	void CycleActive () { // set active bullet to next in list, if last of list set active to first in list
		if (activeBullet == bulletList [bulletList.Count - 1])	{
			activeBullet = bulletList [0];
			UIBulletSelectScript.SwitchActiveBullet (bulletList, activeBullet, bulletList.IndexOf(activeBullet));
		} 
		else {
			activeBullet = bulletList [bulletList.IndexOf(activeBullet) + 1];
			UIBulletSelectScript.SwitchActiveBullet (bulletList, activeBullet, bulletList.IndexOf(activeBullet)); 
		}
			
	}

	void Update () {

		if (Input.GetButtonDown ("Fire1") && currentBulletNumber < maxBullet) {
			Shoot ();
		}

		if (Input.GetButton ("Fire2")) {
			Teleport();
		} 

		if (Input.GetButton ("ClearBullets") && currentBulletNumber != 0) {
			ClearBullets ();
		}
			
		if (Input.GetButtonDown ("CycleBullets") && currentBulletNumber > 1) {
			CycleActive ();	
		} 
	
		if (Input.GetButtonDown ("FocusBullet") && activeBullet != null) {
			StopCoroutine(UIBulletSelectScript.ScaleDown ());
			UIBulletSelectScript.FocusBullet ();

		}

		if (Input.GetButtonUp ("FocusBullet") && activeBullet !=null) {
			StopCoroutine (UIBulletSelectScript.ScaleUp ());
			UIBulletSelectScript.UnfocusBullet();
		}
	}
}