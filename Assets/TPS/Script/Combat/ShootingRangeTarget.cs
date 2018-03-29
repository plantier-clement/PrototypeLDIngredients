using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeTarget : Destructible {

	[SerializeField] 
	float rotationSpeed;

	[SerializeField] 
	float repairingTime;

	Quaternion initialRotation;
	Quaternion targetRotation;

	bool requiresRotation;

	void Awake(){
		initialRotation = transform.rotation;
	
	}

	public override void Die(){
		base.Die ();

		targetRotation = Quaternion.Euler (transform.right * 90);
		requiresRotation = true;

		GameManager.Instance.Timer.Add (() => {
			targetRotation = initialRotation;
			requiresRotation = true;
		}, repairingTime);

	}


	void Update(){
		if (!requiresRotation)
			return;

		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

		if (transform.rotation == targetRotation)
			requiresRotation = false;
		
	}
}
