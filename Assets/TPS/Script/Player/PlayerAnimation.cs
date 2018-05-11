using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {


	public float Vertical;
	public float Horizontal;
	public bool IsWalking;
	public bool IsSprinting;
	public bool IsCrouched;
	public float AimAngle;
	public bool IsAiming;
	public bool IsInCover;
	public bool Reload;

	Animator animator;


	private Player m_Player;
	private Player Player{
		get{ 
			if (m_Player == null)
				m_Player = GetComponent <Player> ();
			return m_Player;
		}
	}

	private PlayerAim m_PlayerAim;
	private PlayerAim PlayerAim{
		get {
			if (m_PlayerAim == null)
				m_PlayerAim = GameManager.Instance.LocalPlayer.playerAim;
			return m_PlayerAim;
		}
	}

	void Awake(){
		animator = GetComponentInChildren<Animator> ();

	}


	void Update(){

		if (GameManager.Instance.IsPaused)
			return;


		if (Player.IsLocalPlayer)
			GetLocalPlayerInput ();

		animator.SetFloat ("Horizontal", Horizontal);
		animator.SetFloat ("Vertical", Vertical);

		animator.SetBool ("IsWalking", IsWalking);
		animator.SetBool ("IsSprinting", IsSprinting);
		animator.SetBool ("IsCrouching", IsCrouched);

		animator.SetFloat ("AimAngle", AimAngle);
		animator.SetBool ("IsAiming", 	IsAiming);

		animator.SetBool ("IsInCover", IsInCover);
		animator.SetBool ("IsReloading", Reload);

	}



	void GetLocalPlayerInput(){

		Vertical = Player.InputState.Vertical;
		Horizontal = Player.InputState.Horizontal;

		IsWalking = GameManager.Instance.LocalPlayer.PlayerState.MoveState == PlayerState.EMoveState.WALKING;
		IsSprinting = Player.InputState.IsSprinting;
		IsCrouched = Player.InputState.IsCrouched;

		AimAngle = PlayerAim.GetAngle ();
		IsAiming = 	GameManager.Instance.LocalPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMING ||
					GameManager.Instance.LocalPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMEDFIRING;
		
		IsInCover = GameManager.Instance.LocalPlayer.PlayerState.MoveState == PlayerState.EMoveState.COVER;

		Reload = Player.WeaponController.ActiveWeapon.Reloader.IsReloading;
	}
}
