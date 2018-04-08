﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {


	InputController.InputState playerInput;


	private Player m_Player;
	public Player Player{
		get { 
			if (m_Player == null)
				m_Player = GetComponent <Player> ();
			return m_Player;
		}
	}

	private CharacterController m_MoveController;
	public CharacterController MoveController {
		get { 
			if (m_MoveController == null)
				m_MoveController = GetComponent <CharacterController> ();
			return m_MoveController;
		}

	}


	void Awake () {
		playerInput = GameManager.Instance.InputController.State;	
	}


	void Update(){

		if (!Player.PlayerHealth.IsAlive || GameManager.Instance.IsPaused)
			return;
		
		Move ();
	}


	public void SetInputController (InputController.InputState state){
	
		playerInput = state;
	}


	public void Move(float horizontal, float vertical){
	
		float moveSpeed = Player.Settings.RunSpeed;

		if (playerInput.IsWalking)
			moveSpeed = Player.Settings.WalkSpeed;

		if (playerInput.IsSprinting)
			moveSpeed = Player.Settings.SprintSpeed;

		if (playerInput.IsCrouched)
			moveSpeed = Player.Settings.CrouchSpeed;

		if (Player.PlayerState.MoveState == PlayerState.EMoveState.COVER)
			moveSpeed = Player.Settings.WalkSpeed;


		Vector2 direction = new Vector2 (vertical * moveSpeed, horizontal * moveSpeed);
		MoveController.SimpleMove (transform.forward * direction.x + transform.right * direction.y );
	
	}

	void Move () {


		if (!Player.IsLocalPlayer)
			return;

		if (playerInput == null) {
			
			playerInput = GameManager.Instance.InputController.State;
			if (playerInput == null)
				return;
		}
		if(!GameManager.Instance.IsNetworkGame)
			Move (playerInput.Horizontal, playerInput.Vertical);
	}



}
