using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgOverTime : MonoBehaviour {


	public float damageAmount;
	public float damageTotal;
	public float periodInSeconds;
//	public bool blabla; // si sors du trigger, one last set of dmg?

	// PRIVATE
	HealthManager refHM;
//	bool isTakingDamage;

	void OnTriggerEnter (Collider coll){
		if (coll.gameObject.tag == "Player") {
			refHM = coll.GetComponent<HealthManager> ();
			StartCoroutine (refHM.DoDamageOTime (damageAmount, damageTotal, periodInSeconds));
		}
	}

	void OnTriggerExit (Collider coll){
		if (coll.gameObject.tag == "Player") {
			StopCoroutine (refHM.DoDamageOTime (damageAmount, damageTotal, periodInSeconds));
		}
	}



	/*
	IEnumerator SetOnFire (float periodInSeconds){
		float currentCount = 0;
		while
		{
			currentHealth -= damageAmount;
			yield return new WaitForSeconds(1);
			currentCount += damageTotal;
		}
	}
*/

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
