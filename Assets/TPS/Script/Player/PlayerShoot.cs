using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

	[SerializeField] float weaponSwitchTime;

	Shooter[] weapons;
	Shooter activeWeapon;

	int currentWeaponIndex = 0;
	bool canFire;
	Transform weaponHolster;

	public event System.Action<Shooter> OnWeaponSwitch;

	public Shooter ActiveWeapon{
		get{
			return activeWeapon;
		}
	}

	void Awake(){
		canFire = true;
		weaponHolster = transform.Find ("Weapons");
		weapons = weaponHolster.GetComponentsInChildren<Shooter> ();

		if (weapons.Length > 0)
			Equip (0);
	}

	void DeactivateWeapons(){
		for (int i = 0; i < weapons.Length; i++) {
			weapons [i].gameObject.SetActive (false);
			weapons [i].transform.SetParent (weaponHolster);

		}
		
	}
		
	void SwitchWeapon (int direction){

		canFire = false;
		currentWeaponIndex += direction;

		if (currentWeaponIndex > weapons.Length - 1)
			currentWeaponIndex = 0;

		if (currentWeaponIndex < 0)
			currentWeaponIndex = weapons.Length - 1;

		GameManager.Instance.Timer.Add (() => {
			Equip (currentWeaponIndex);
		}, weaponSwitchTime);

	}

	void Equip(int index){
		DeactivateWeapons ();
		canFire = true;
		activeWeapon = weapons [index];
		activeWeapon.Equip ();
		weapons [index].gameObject.SetActive (true);

		if (OnWeaponSwitch != null)
			OnWeaponSwitch (activeWeapon);
	}


	void Update(){

		if (GameManager.Instance.InputController.MouseWheelDown)
			SwitchWeapon (1);

		if (GameManager.Instance.InputController.MouseWheelUp)
			SwitchWeapon (-1);

		if (GameManager.Instance.LocalPlayer.PlayerState.MoveState == PlayerState.EMoveState.SPRINTING)
			return;

		if (!canFire)
			return;

		if (GameManager.Instance.InputController.Fire1) {
			activeWeapon.Fire ();
		}
	}
}
