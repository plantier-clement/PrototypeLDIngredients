using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

	[SerializeField] float rateOfFire;
	[SerializeField] Projectile projectile;
	[SerializeField] public Transform hand;
	[SerializeField] AudioControllerRandom audioFire;
	[SerializeField] GameObject BulletCase;
	[SerializeField] float caseExpelForce;

	[HideInInspector]
	public Vector3 AimPoint;
	[HideInInspector]
	public Vector3 AimTargetOffset;
	[HideInInspector]
	public WeaponReloader Reloader;
	[HideInInspector]
	public float StartingOrientation;

	[HideInInspector]
	public bool CanFire;

	Player player;
	float nextFireAllowed;

	Transform muzzle;
	private ParticleSystem muzzleParticleSystem;

	Transform bulletExtractor;
	private ParticleSystem extractorParticleSystem;


	private WeaponRecoil m_WeaponRecoil;
	private WeaponRecoil WeaponRecoil{
		get{ 
			if (m_WeaponRecoil == null)
				m_WeaponRecoil = GetComponent <WeaponRecoil> ();
			return m_WeaponRecoil;
		}
	}

	PlayerAim m_PlayerAim;
	PlayerAim PlayerAim
	{
		get {
			if(m_PlayerAim == null)
				m_PlayerAim = GameManager.Instance.LocalPlayer.GetComponentInChildren <PlayerAim> ();
			return m_PlayerAim;
		}
	}


	void Awake () {
		player = GetComponentInParent<Player> ();
		Reloader = GetComponent<WeaponReloader> ();

		muzzle = transform.Find ("Model/Muzzle");
		bulletExtractor = transform.Find ("Model/BulletExtractor");
		muzzleParticleSystem = muzzle.GetComponent <ParticleSystem> ();
		extractorParticleSystem = bulletExtractor.GetComponent <ParticleSystem> ();
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
			
		PlayFireEffects ();
		CanFire = true;

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
	}


	void PlayMuzzleFlash(){
		if (muzzleParticleSystem == null)
			return;
		muzzleParticleSystem.Play ();
	}


	void PlaySmokeEffect(){
		if (extractorParticleSystem == null)
			return;
		extractorParticleSystem.Play ();
	}


	void PlayBulletCaseEffect(){
		GameObject newCase =   Instantiate (BulletCase, bulletExtractor.position, bulletExtractor.rotation);
		Rigidbody rb = newCase.GetComponent<Rigidbody>();
		rb.AddForce(bulletExtractor.transform.forward * caseExpelForce);
	}


	public virtual void PlayFireEffects(){
		PlayMuzzleFlash ();
		audioFire.Play ();
		PlayBulletCaseEffect ();
		//	PlaySmokeEffect ();
	}


	public void SetAimPoint(Vector3 target){
		AimPoint = target;
	}
}
