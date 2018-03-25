using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AmmoCounter : MonoBehaviour {

	[SerializeField] Text text;

	PlayerShoot playerShoot;
	WeaponReloader reloader;


	void Awake () {
		GameManager.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;

	}

	void HandleOnLocalPlayerJoined (Player player){
		playerShoot = player.PlayerShoot;
		playerShoot.OnWeaponSwitch += HandleOnWeaponSwitch;
	
	}

	void HandleOnWeaponSwitch (Shooter activeWeapon)
	{
		reloader = activeWeapon.Reloader;
		reloader.OnAmmoChanged += HandleOnAmmoChanged;
		HandleOnAmmoChanged ();
	}



	void HandleOnAmmoChanged(){
		int amountInInventory = reloader.RoundsRemainingInInventory;
		int amountInClip = reloader.RoundsRemainingInClip;
		text.text = string.Format ("{0} / {1}", amountInClip, amountInInventory);

	}


}
