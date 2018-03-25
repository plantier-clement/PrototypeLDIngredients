using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	public float Vertical;
	public float Horizontal;
	public Vector2 MouseInput;
	public bool Fire1;
	public bool Aim;
	public bool Reload;
	public bool IsWalking;
	public bool IsSprinting;
	public bool IsCrouched;
	public bool MouseWheelUp;
	public bool MouseWheelDown;




	void Update () {
		Vertical = Input.GetAxis ("Vertical");
		Horizontal = Input.GetAxis ("Horizontal");
		MouseInput = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		Fire1 = Input.GetButton ("Fire1");
		Aim = Input.GetButton ("Aim");
		Reload = Input.GetButton("Reload");
		IsWalking = Input.GetButton ("Walk");
		IsSprinting = Input.GetButton ("Sprint");
		IsCrouched = Input.GetButton ("Crouch");
		MouseWheelUp = Input.GetAxis("Mouse ScrollWheel") > 0;
		MouseWheelDown = Input.GetAxis("Mouse ScrollWheel") < 0;

	}
}
