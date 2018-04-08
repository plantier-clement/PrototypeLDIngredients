using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

	[SerializeField] float rateOfFire;
	[SerializeField] Projectile projectile;
	[SerializeField] public Transform hand;
	[SerializeField] AudioController audioReload;
	[SerializeField] AudioController audioFire;

	public Vector3 AimPoint;
	public Vector3 AimTargetOffset;
	public WeaponReloader Reloader;

	[HideInInspector]
	public bool CanFire;

	Player player;
	private ParticleSystem muzzleParticleSystem;
	float nextFireAllowed;
	Transform muzzle;

	private WeaponRecoil m_WeaponRecoil;
	private WeaponRecoil WeaponRecoil{
		get{ 
			if (m_WeaponRecoil == null)
				m_WeaponRecoil = GetComponent <WeaponRecoil> ();
			return m_WeaponRecoil;
		}
	}


	public void SetAimPoint(Vector3 target){
		AimPoint = target;

	}


	void Awake () {
		muzzle = transform.Find ("Model/Muzzle");
		Reloader = GetComponent<WeaponReloader> ();
		muzzleParticleSystem = muzzle.GetComponent < ParticleSystem> ();
		player = GetComponentInParent <Player> ();
	}
		

	public void Equip(){
		transform.SetParent (hand);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}


	void OnDisable(){
		//
	}
		

	public void Reload(){
		if (Reloader == null)
			return;

		if(player.IsLocalPlayer)
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

		if (player.IsLocalPlayer && Reloader != null) {
			
			if (Reloader.IsReloading)
				return;
			
			if (Reloader.RoundsRemainingInClip == 0)
				return;
			
			Reloader.TakeFromClip (1);
		}

		nextFireAllowed = Time.time + rateOfFire;

		muzzle.LookAt (AimPoint + AimTargetOffset);
		Projectile newBullet = Instantiate (projectile, muzzle.position, muzzle.rotation);


		if (this.WeaponRecoil)
			this.WeaponRecoil.Activate ();
			
		FireEffect ();
		audioFire.Play ();
		CanFire = true;
	}

}
