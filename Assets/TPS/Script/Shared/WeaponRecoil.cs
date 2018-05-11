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

	[SerializeField]
	Layer[] layers;

	[SerializeField]
	float recoilSpeed;

	[SerializeField]
	float recoilCooldown;

	[SerializeField]
	float recoilStrength;

	[SerializeField]
	float recoilCamOffset;

	float nextRecoilCooldown;
	float recoilActiveTime;

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


	void Update(){
		if (nextRecoilCooldown > Time.time) {
			// holding fire button

			recoilActiveTime += Time.deltaTime;
			float percentage = GetPercentage ();
	

			Vector3 recoilAmount = Vector3.zero;
			for (int i = 0; i < layers.Length; i++)
				recoilAmount += layers [i].direction * layers [i].curve.Evaluate (percentage);

			this.Shooter.AimTargetOffset = Vector3.Lerp (Shooter.AimTargetOffset, Shooter.AimTargetOffset + recoilAmount, recoilStrength * Time.deltaTime);
			this.Crosshair.ApplyScale (percentage * Random.Range (recoilStrength * 7, recoilStrength * 9));
			this.PlayerAim.SetRotation (recoilCamOffset);

		} else {
			// not holding fire button
			recoilActiveTime -= Time.deltaTime;
			if (recoilActiveTime < 0)
				recoilActiveTime = 0;

			this.Crosshair.ApplyScale (GetPercentage ());

			// reset
			if (recoilActiveTime == 0) {
				this.Shooter.AimTargetOffset = Vector3.zero;
				this.Crosshair.ApplyScale (0);
			}
		}
	
	}


	public void Activate (){
		nextRecoilCooldown = Time.time + recoilCooldown;
	}


	float GetPercentage(){
		float percentage = recoilActiveTime / recoilSpeed;
		return Mathf.Clamp01 (percentage);
	
	}


}
