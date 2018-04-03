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

	[SerializeField] SwatSoldier settings;
	[SerializeField] MouseInput MouseControl;
	[SerializeField] AudioController footsteps;
	[SerializeField] float minimumMoveThreshold;

	public PlayerAim playerAim;

	private Vector3 previousPosition;

	private CharacterController m_MoveController;
	public CharacterController MoveController{
		get{
			if (m_MoveController == null)
				m_MoveController = GetComponent <CharacterController> ();
			return m_MoveController;
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

	InputController playerInput;
	Vector2 mouseInput;

	void Awake() {
		playerInput = GameManager.Instance.InputController;
		GameManager.Instance.LocalPlayer = this;
		if (MouseControl.LockMouse) {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	void Move(){
		float moveSpeed = settings.RunSpeed;
		if (playerInput.IsWalking)
			moveSpeed = settings.WalkSpeed;

		if (playerInput.IsSprinting)
			moveSpeed = settings.SprintSpeed;

		if (playerInput.IsCrouched)
			moveSpeed = settings.CrouchSpeed;

		Vector2 direction = new Vector2 (playerInput.Vertical * moveSpeed, playerInput.Horizontal * moveSpeed);

		MoveController.SimpleMove (transform.forward * direction.x + transform.right * direction.y );



		if (Vector3.Distance(transform.position, previousPosition) > minimumMoveThreshold)
			footsteps.Play ();
		

		previousPosition = transform.position;
	}

	void LookAround ()
	{
		mouseInput.x = Mathf.Lerp (mouseInput.x, playerInput.MouseInput.x, 1.0f / MouseControl.Damping.x);
		mouseInput.y = Mathf.Lerp (mouseInput.y, playerInput.MouseInput.y, 1.0f / MouseControl.Damping.y);
		transform.Rotate (Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);

		playerAim.SetRotation (mouseInput.y * MouseControl.Sensitivity.y);

	}

	void Update () {

		if (!PlayerHealth.IsAlive)
			return;
		
		Move ();
		LookAround ();
	}
}
