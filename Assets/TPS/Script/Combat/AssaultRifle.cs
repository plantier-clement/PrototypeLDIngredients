using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Shooter {


	public override void Fire ()
	{
		base.Fire ();
		if (CanFire) {

		}
	}

	public void Update(){
		if (GameManager.Instance.InputController.Reload) {
			Reload ();
		}
	}
}