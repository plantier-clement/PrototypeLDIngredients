using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerHealth))]

public class Player : MonoBehaviour {

	[System.Serializable]
	public class MouseInput {
		public Vector2 Damping;
		public Vector2 Sensitivity;
		public bool LockMouse;
	}

	[SerializeField] MouseInput MouseControl;
	[SerializeField] AudioControllerRandom footsteps;
	[SerializeField] float minimumMoveThreshold;

	public SwatSoldier Settings;
	public PlayerAim playerAim;
	public bool IsLocalPlayer;


	InputController playerInput;
	Vector2 mouseInput;


	private CharacterController m_MoveController;
	public CharacterController MoveController{
		get{
			if (m_MoveController == null)
				m_MoveController = GetComponent <CharacterController> ();
			return m_MoveController;
		}
	}

	private InputController.InputState m_InputState;
	public InputController.InputState InputState {
		get { 
			if (m_InputState == null)
				m_InputState = GameManager.Instance.InputController.State;
			return m_InputState;
		}
	}


	private PlayerShoot m_PlayerShoot;
	public PlayerShoot PlayerShoot{
		get {
			if (m_PlayerShoot == null)
				m_PlayerShoot = GetComponent<PlayerShoot> ();
			return m_PlayerShoot;
		}
	}

	private WeaponController m_WeaponController;
	public WeaponController WeaponController {
		get {
			if (m_WeaponController == null)
				m_WeaponController = GetComponent<WeaponController> ();
			return m_WeaponController;
		}
	}

	private PlayerHealth m_PlayerHealth;
	public PlayerHealth PlayerHealth{
		get {
			if (m_PlayerHealth == null)
				m_PlayerHealth = GetComponent<PlayerHealth> ();
			return m_PlayerHealth;
		}
	}


	private PlayerState m_PlayerState;
	public PlayerState PlayerState{
		get{
			if (m_PlayerState == null)
				m_PlayerState = GetComponent<PlayerState> ();
			return m_PlayerState;
		}
	}


	void Awake() {
		
		if(!GameManager.Instance.IsNetworkGame)
			SetAsLocalPlayer ();
	}


	void Update () {

		if (!PlayerHealth.IsAlive || GameManager.Instance.IsPaused)
			return;

		if (IsLocalPlayer) {
			LookAround ();
		}
	}


	public void SetInputState (InputController.InputState state) {
	
		m_InputState = state;
	}


	public void SetAsLocalPlayer(){

		IsLocalPlayer = true;
		playerInput = GameManager.Instance.InputController;
		GameManager.Instance.LocalPlayer = this;

		SetCursor (MouseControl.LockMouse);
	
	}


	public void SetCursor(bool value) {
		
		Cursor.visible = !value;
		Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
	}


	void LookAround () {

		mouseInput.x = Mathf.Lerp (mouseInput.x, playerInput.MouseInput.x, 1.0f / MouseControl.Damping.x);
		mouseInput.y = Mathf.Lerp (mouseInput.y, playerInput.MouseInput.y, 1.0f / MouseControl.Damping.y);
		transform.Rotate (Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);

		playerAim.SetRotation (mouseInput.y * MouseControl.Sensitivity.y);
	}
}
