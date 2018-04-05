using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour {

	[SerializeField] 
	Collider trigger;

	PlayerCover playerCover;


	void OnTriggerEnter (Collider other){
	
		if (!CheckLocalPlayer (other))
			return;
	
		playerCover.SetPlayerCoverAllowed (true);

	}

	void OnTriggerExit (Collider other){
		if (!CheckLocalPlayer (other))
			return;
	
		playerCover.SetPlayerCoverAllowed (false);

	}

	bool CheckLocalPlayer(Collider other){

		if (other.tag != "Player")
			return false;

		if (other.GetComponent <Player> () != GameManager.Instance.LocalPlayer)
			return false;

		playerCover = GameManager.Instance.LocalPlayer.GetComponent <PlayerCover>();
		return true;

	}
}
