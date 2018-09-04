using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	[SerializeField] 
	float weaponSwitchTime;

	[HideInInspector]
	public bool CanFire;


	Shooter[] weapons;
	int currentWeaponIndex = 0;
	Transform weaponHolster;

	public event System.Action<Shooter> OnWeaponSwitch;

	Shooter m_ActiveWeapon;
	public Shooter ActiveWeapon{
		get{
			return m_ActiveWeapon;
		}
	}


	void Awake(){
		CanFire = true;
		weaponHolster = transform.Find ("Weapons");
		weapons = weaponHolster.GetComponentsInChildren<Shooter> ();

		if (weapons.Length > 0)
			Equip (0);
	}


	public Vector3 GetImpactPoint(){

		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (.5f, .5f, 0));
		RaycastHit hit;
	//	Vector3 targetPosition = ray.GetPoint (500);

		if (Physics.Raycast (ray, out hit))
			return hit.point;
	
		return transform.position + transform.forward * 50;
	}


	void DeactivateWeapons(){
		for (int i = 0; i < weapons.Length; i++) {
			weapons [i].gameObject.SetActive (false);
			weapons [i].transform.SetParent (weaponHolster);
		}
	}


	internal void SwitchWeapon (int direction){

		CanFire = false;
		currentWeaponIndex += direction;

		if (currentWeaponIndex > weapons.Length - 1)
			currentWeaponIndex = 0;

		if (currentWeaponIndex < 0)
			currentWeaponIndex = weapons.Length - 1;

		GameManager.Instance.Timer.Add (() => {
			Equip (currentWeaponIndex);
		}, weaponSwitchTime);

	}


	internal void Equip(int index){
		DeactivateWeapons ();
		CanFire = true;
		m_ActiveWeapon = weapons [index];
		m_ActiveWeapon.Equip ();
		weapons [index].gameObject.SetActive (true);

		if (OnWeaponSwitch != null)
			OnWeaponSwitch (m_ActiveWeapon);
	}
}
