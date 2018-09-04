using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	[System.Serializable]
	public class InputState
	{
		public float Vertical;
		public float Horizontal;
		public bool Fire1;
		public bool Aim;
		public bool Reload;
		public bool IsWalking;
		public bool IsSprinting;
		public bool IsCrouched;
		public bool CoverToggle;
		public bool IsInCover;
		public float AimAngle;
		public bool IsAiming;
	}

	public InputState State;

	public float Vertical {	get { return State.Vertical; } }
	public float Horizontal { get { return State.Horizontal; } }
	public float AimAngle { get { return State.AimAngle; } }
	public bool Fire1 {	get { return State.Fire1; } }
	public bool Aim { get { return State.Aim; } }
	public bool Reload { get { return State.Reload; } }
	public bool IsWalking {	get { return State.IsWalking; } }
	public bool IsSprinting { get { return State.IsSprinting; } }
	public bool IsCrouched { get { return State.IsCrouched; } }
	public bool CoverToggle { get { return State.CoverToggle; } }
	public bool IsInCover { get { return State.IsInCover; } }

	public Vector2 MouseInput;
	public bool MouseWheelUp;
	public bool MouseWheelDown;
	public bool Escape;


	void Start (){
	
		State = new InputState ();
	}


	void Update () {
		
		State.Vertical = Input.GetAxis ("Vertical");
		State.Horizontal = Input.GetAxis ("Horizontal");
		State.Fire1 = Input.GetButton ("Fire1");
		State.Aim = Input.GetButton ("Aim");
		State.Reload = Input.GetButton("Reload");
		State.IsWalking = Input.GetButton ("Walk");
		State.IsSprinting = Input.GetButton ("Sprint");
		State.IsCrouched = Input.GetButton ("Crouch");
		State.CoverToggle = Input.GetButtonDown ("Cover");

		MouseInput = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		MouseWheelUp = Input.GetAxis("Mouse ScrollWheel") > 0;
		MouseWheelDown = Input.GetAxis("Mouse ScrollWheel") < 0;
		Escape = Input.GetButton ("Cancel");

	}


}
