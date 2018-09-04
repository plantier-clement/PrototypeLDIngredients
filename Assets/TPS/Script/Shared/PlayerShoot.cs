using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerShoot : WeaponController {

	bool IsPlayerAlive = true;
	Player player;


	void Start(){

		IsPlayerAlive = true;
		player = GetComponent <Player> ();
		player.PlayerHealth.OnDeath += PlayerHealth_OnDeath;
	}


	void Update(){

		if (!player.IsLocalPlayer && IsPlayerAlive) {
		
			if (player.InputState.Fire1) {
				ActiveWeapon.Fire ();
			} 
		}
	

		if (!IsPlayerAlive || GameManager.Instance.IsPaused)
			return;

		if (player.IsLocalPlayer) {

			if (GameManager.Instance.InputController.MouseWheelDown)
				SwitchWeapon (1);

			if (GameManager.Instance.InputController.MouseWheelUp)
				SwitchWeapon (-1);

			if (GameManager.Instance.LocalPlayer.PlayerState.MoveState == PlayerState.EMoveState.SPRINTING)
				return;

			if (!CanFire)
				return;

			if (player.InputState.Fire1) {
				ActiveWeapon.SetAimPoint (GetImpactPoint ());
				ActiveWeapon.Fire ();
			}

			if (player.InputState.Reload)
				ActiveWeapon.Reload ();
		}
	}


	private void PlayerHealth_OnDeath(){

		IsPlayerAlive = false;
	}
}
