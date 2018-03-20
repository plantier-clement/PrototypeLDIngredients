﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {


	void OnTriggerEnter (Collider other){
		if (other.gameObject.tag == "Player") {
			HealthManager refHM = other.GetComponent<HealthManager> ();
			refHM.Kill (other.gameObject);
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
