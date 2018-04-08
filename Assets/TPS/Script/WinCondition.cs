using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour {


	[SerializeField]
	Destructible[] targets;

	int targetsDestroyedCounter;

	void Start() {
	
		for (int i = 0; i < targets.Length; i++) {

			targets[i].OnDeath += WinCondition_OnDeath;
		}
	
	}


	void WinCondition_OnDeath () {
		targetsDestroyedCounter++;
		if (targetsDestroyedCounter == targets.Length)
			GameManager.Instance.EventBus.RaiseEvent ("OnAllEnemiesKilled");
	}


}
