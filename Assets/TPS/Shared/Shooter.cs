using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

	[SerializeField] float rateOfFire;
	[SerializeField] Projectile projectile;
	[SerializeField] public Transform hand;
	[SerializeField] AudioController audioReload;
	[SerializeField] AudioController audioFire;

	public Transform AimTarget;
	public Vector3 AimTargetOffset;

	public WeaponReloader Reloader;
	private ParticleSystem muzzleParticleSystem;

	float nextFireAllowed;
	Transform muzzle;

	[HideInInspector]
	public bool CanFire;

	public void Equip(){
		transform.SetParent (hand);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	void OnDisable(){
		//
	}

	void Awake () {
		muzzle = transform.Find ("Model/Muzzle");
		Reloader = GetComponent<WeaponReloader> ();
		muzzleParticleSystem = muzzle.GetComponent < ParticleSystem> ();
	}

	public void Reload(){
		if (Reloader == null)
			return;
		Reloader.Reload();
		audioReload.Play ();
	}

	void FireEffect(){
		if (muzzleParticleSystem == null)
			return;
		muzzleParticleSystem.Play ();
	}

	public virtual void Fire(){

		CanFire = false;

		if (Time.time < nextFireAllowed)
			return;

		if (Reloader != null) {
			if (Reloader.IsReloading)
				return;
			if (Reloader.RoundsRemainingInClip == 0)
				return;
			Reloader.TakeFromClip (1);
		}

		nextFireAllowed = Time.time + rateOfFire;

		muzzle.LookAt (AimTarget.position + AimTargetOffset);
		FireEffect ();


		Instantiate (projectile, muzzle.position, muzzle.rotation);
		audioFire.Play ();
		CanFire = true;
	}

}
