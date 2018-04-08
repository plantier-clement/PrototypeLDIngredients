using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedExtensions;


[RequireComponent(typeof(EnemyPlayer))]
public class EnemyShoot : WeaponController {

	[SerializeField]
	float shootingSpeed;

	[SerializeField]
	float burstDurationMax;

	[SerializeField]
	float burstDurationMin;

	EnemyPlayer enemyPlayer;
	bool shouldFire;

	EnemyAnimation enemyAnimation;

	Vector3 myTarget;


	void Start() {
		enemyPlayer = GetComponent <EnemyPlayer> ();
		enemyPlayer.OnTargetSelected += EnemyPlayer_OnTargetSelected;
		enemyAnimation = enemyPlayer.GetComponent <EnemyAnimation>();

	}


	void Update () {
		if (!shouldFire || !CanFire || !enemyPlayer.EnemyHealth.IsAlive)
			return;
		ActiveWeapon.Fire ();
	}


	void EnemyPlayer_OnTargetSelected (Player target) {

		myTarget = target.transform.position;
		ActiveWeapon.SetAimPoint (target.transform.position);
		ActiveWeapon.AimTargetOffset = Vector3.up * 1.5f;
		StartBurst ();
	}


	void CrouchState(){

		bool takeCover = Random.Range (0, 3) == 0;  // possibilities = 0, 1, 2, 3 ; if 0 then do something. basically 25% chance

		if (!takeCover)
			return;

		float distanceToTarget = Vector3.Distance (transform.position, myTarget);
		if (distanceToTarget > 15)
			enemyAnimation.IsCrouched = true;
	}


	void StartBurst (){

		if (!enemyPlayer.EnemyHealth.IsAlive && !CanSeeTarget ())
			return;

		CheckReload ();
		CrouchState ();
		shouldFire = true;

		GameManager.Instance.Timer.Add (EndBurst, Random.Range (burstDurationMin, burstDurationMax));
	}


	void EndBurst (){
		shouldFire = false;
		if (!enemyPlayer.EnemyHealth.IsAlive)
			return;
		
		CheckReload ();
		CrouchState ();

		if(CanSeeTarget ())
			GameManager.Instance.Timer.Add (StartBurst, shootingSpeed);
	}


	bool CanSeeTarget(){

		if (!transform.IsInLineOfSight (myTarget, 90, enemyPlayer.playerScanner.mask, Vector3.up)) {
			// clear the target
			enemyPlayer.ClearTargetAndScan();
			return false;
		}
		return true;
	}


	void CheckReload (){

		if (ActiveWeapon.Reloader.RoundsRemainingInClip == 0)
			ActiveWeapon.Reload ();

	}



}
