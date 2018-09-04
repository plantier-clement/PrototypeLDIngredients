using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class WeaponRecoil : MonoBehaviour {


	[System.Serializable]
	public struct Layer
	{
		public AnimationCurve curve;
		public Vector3 direction;
	}


	[System.Serializable]
	public class CrosshairSettings {
		public float MaxSpread;
		public float SpeedToMaxSpread;
		public float SpeedToMinSpread;
	}

	[SerializeField] Layer[] layers;
	[SerializeField] CrosshairSettings CrosshairWhileRunning;
	[SerializeField] CrosshairSettings CrosshairWhileCrouching;
	[SerializeField] CrosshairSettings CrosshairWhileLooking;
	[SerializeField] float recoilCooldown;
	[SerializeField] float recoilCamOffset;

	float nextRecoilCooldown;
	float recoilActiveTime;
	float startingOrientation;
	bool recoilActive = false;


	Shooter m_Shooter;
	Shooter Shooter
	{
		get {
			if(m_Shooter == null)
				m_Shooter = GetComponent <Shooter> ();
			return m_Shooter;
		}
	}

	Crosshair m_Crosshair;
	Crosshair Crosshair
	{
		get {
			if(m_Crosshair == null)
				m_Crosshair = GameManager.Instance.LocalPlayer.playerAim.GetComponentInChildren <Crosshair> ();
			return m_Crosshair;
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

	PlayerState m_PlayerState;
	PlayerState PlayerState
	{
		get {
			if(m_PlayerState == null)
				m_PlayerState = GameManager.Instance.LocalPlayer.GetComponentInChildren <PlayerState> ();
			return m_PlayerState;
		}
	}

	private InputController m_PlayerInput;
	public InputController PlayerInput {
		get { 
			if (m_PlayerInput == null)
				m_PlayerInput = GameManager.Instance.InputController;
			return m_PlayerInput;
		}
	}


	void Update(){

		float maxSpread;
		float speedToMaxSpread;
		float speedToMinSpread;


		float camSpread = 0.0f;;
		float moveSpread;

		// conditions
		moveSpread = CrosshairWhileRunning.MaxSpread;
		speedToMaxSpread = CrosshairWhileRunning.SpeedToMaxSpread;
		speedToMinSpread = CrosshairWhileRunning.SpeedToMinSpread;

			
		if (this.PlayerState.MoveState == PlayerState.EMoveState.CROUCHING) {
			moveSpread = CrosshairWhileCrouching.MaxSpread;
			speedToMaxSpread = CrosshairWhileCrouching.SpeedToMaxSpread;
			speedToMinSpread = CrosshairWhileCrouching.SpeedToMinSpread;
		}

		if (PlayerInput.MouseInput != Vector2.zero) {
			camSpread = CrosshairWhileLooking.MaxSpread;
		}

		maxSpread = moveSpread + camSpread;

		if(recoilActive) {
			if (nextRecoilCooldown > Time.time) {
				// holding fire button
				recoilActiveTime += Time.deltaTime; 
				recoilActiveTime = Mathf.Clamp (recoilActiveTime, 0f, 0.8f);
				print (recoilActiveTime);

				Vector3 recoilAmount = Vector3.zero;
				for (int i = 0; i < layers.Length; i++)
					recoilAmount += layers [i].direction * layers [i].curve.Evaluate (GetPercentage (speedToMaxSpread));

			//	this.Shooter.AimTargetOffset = Vector3.Lerp (Shooter.AimTargetOffset, Shooter.AimTargetOffset + recoilAmount, recoilStrength * Time.deltaTime);
				this.Crosshair.ApplyScale (GetPercentage (speedToMaxSpread) * maxSpread);
				this.PlayerAim.SetRotation (recoilCamOffset);

			} else {
				// not holding fire button

				print (recoilActiveTime);
				recoilActiveTime -= Time.deltaTime;

				this.Crosshair.ApplyScale (GetPercentage (speedToMinSpread) * maxSpread);

				if (recoilActiveTime < 0)
					recoilActiveTime = 0;
				// reset
				if (recoilActiveTime == 0) {
				//	this.Shooter.AimTargetOffset = Vector3.zero;
					this.Crosshair.ApplyScale (0);
					recoilActive = false;
				}
			}
		}
	}


	public void Activate (){
		nextRecoilCooldown = Time.time + recoilCooldown;
		recoilActive = true;
	}


	float GetPercentage(float speed){
		float percentage = recoilActiveTime / speed;
		return Mathf.Clamp01 (percentage);
	
	}


}
