using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloader : MonoBehaviour {

	[SerializeField] int maxAmmo;
	[SerializeField] float reloadTime;
	[SerializeField] int clipSize;
	[SerializeField] Container inventory;
	[SerializeField] EWeaponType weaponType;	
	[SerializeField] AudioController audioReload;
	[SerializeField] AudioClip reloadClipStart;
	[SerializeField] AudioClip reloadClipEnd;

	[HideInInspector]
	public int shotsFiredInClip;
	int ammo;
	bool isReloading;
	System.Guid containerItemId;

	public event System.Action OnAmmoChanged;


	public bool CurrentClipIsFull {
		get {
			return RoundsRemainingInClip == clipSize;
		}
	}

	public int RoundsRemainingInClip{
		get{
			return clipSize - shotsFiredInClip;
		}
	}

	public int RoundsRemainingInInventory{
		get{
			return inventory.GetAmountRemaining(containerItemId);
		}
	}

	public bool IsReloading {
		get {
			return isReloading;
		}
	}


	void Awake(){
		inventory.OnContainerReady += () => {
			containerItemId = inventory.Add (weaponType.ToString (), maxAmmo);
		};
	}
		

	public void Reload(){
		if (isReloading || CurrentClipIsFull)
			return;
		
		isReloading = true;
		audioReload.Play (reloadClipStart);
		GameManager.Instance.Timer.Add (() => {
			ExecuteReload(inventory.TakeFromContainer(containerItemId, clipSize - RoundsRemainingInClip));
		}, reloadTime);
	}


	private void ExecuteReload(int amount){
		audioReload.Play (reloadClipEnd);
		isReloading = false;
		shotsFiredInClip -= amount;
		HandleOnAmmoChanged ();
	}


	public void TakeFromClip(int amount){
		shotsFiredInClip += amount;
		HandleOnAmmoChanged ();
	}


	public void HandleOnAmmoChanged(){
		if (OnAmmoChanged != null)
			OnAmmoChanged ();
	}
}
