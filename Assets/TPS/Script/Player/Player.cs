using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MoveController))]
[RequireComponent(typeof(PlayerState))]

public class Player : MonoBehaviour {

	[System.Serializable]
	public class MouseInput {
		public Vector2 Damping;
		public Vector2 Sensitivity;
		public bool LockMouse;
	}

	[SerializeField] float runSpeed;
	[SerializeField] float walkSpeed;
	[SerializeField] float crouchSpeed;
	[SerializeField] float sprintSpeed;
	[SerializeField] MouseInput MouseControl;
	[SerializeField] AudioController footsteps;
	[SerializeField] float minimumMoveThreshold;

	public PlayerAim playerAim;

	private Vector3 previousPosition;

	private MoveController m_MoveController;
	public MoveController MoveController{
		get{
			if (m_MoveController == null)
				m_MoveController = GetComponent <MoveController> ();
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
		float moveSpeed = runSpeed;
		if (playerInput.IsWalking)
			moveSpeed = walkSpeed;

		if (playerInput.IsSprinting)
			moveSpeed = sprintSpeed;

		if (playerInput.IsCrouched)
			moveSpeed = crouchSpeed;

		Vector2 direction = new Vector2 (playerInput.Vertical * moveSpeed, playerInput.Horizontal * moveSpeed);
		MoveController.Move (direction);

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
		Move ();
		LookAround ();
	}
}
