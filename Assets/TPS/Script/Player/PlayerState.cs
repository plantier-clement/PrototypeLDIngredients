using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

	public enum  EMoveState {
		WALKING,
		RUNNING,
		CROUCHING,
		SPRINTING,
		COVER
	}


	public enum  EWeaponState {
		IDLE,
		FIRING,
		AIMING,
		AIMEDFIRING
	}

	bool isInCover = false;

	public EMoveState MoveState;
	public EWeaponState WeaponState;

	private InputController m_InputController;
	public InputController InputController{
		get{
			if (m_InputController == null)
				m_InputController = GameManager.Instance.InputController;
			return m_InputController;
		}
	}

	void Awake(){
		GameManager.Instance.EventBus.AddListener ("CoverToggle", CoverToggle);
	
	}

	void CoverToggle(){
		isInCover = !isInCover;
	}

	void Update (){
		SetMoveState ();
		SetWeaponState ();
	}

	void SetWeaponState(){						
		WeaponState = EWeaponState.IDLE;

		if (InputController.Fire1)
			WeaponState = EWeaponState.FIRING;

		if (InputController.Aim)
			WeaponState = EWeaponState.AIMING;

		if (InputController.Fire1 && InputController.Aim)
			WeaponState = EWeaponState.AIMEDFIRING;
	}


	void SetMoveState(){
		MoveState = EMoveState.RUNNING;

		if (InputController.IsSprinting)
			MoveState = EMoveState.SPRINTING;

		if (InputController.IsWalking)
			MoveState = EMoveState.WALKING;
		
		if (InputController.IsCrouched)
			MoveState = EMoveState.CROUCHING;

		if (isInCover)
			MoveState = EMoveState.COVER;
	
	}

}
