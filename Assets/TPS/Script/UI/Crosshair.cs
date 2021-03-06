﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {
	
	[SerializeField]
	float speed;

	[HideInInspector]
	public Transform Reticle;

	Transform crossTop;
	Transform crossBottom;
	Transform crossLeft;
	Transform crossRight;

	float reticleStartPoint;

	void Start(){

		if (!GetComponentInParent <Player> ().IsLocalPlayer) {
			Destroy (this.gameObject);
			return;
		}
			
		Reticle = GameObject.Find ("Canvas/Reticle").transform;

		crossTop = Reticle.Find ("Cross/Top").transform;
		crossBottom = Reticle.Find ("Cross/Bottom").transform;
		crossLeft = Reticle.Find ("Cross/Left").transform;
		crossRight = Reticle.Find ("Cross/Right").transform;
	
		reticleStartPoint = crossTop.localPosition.y;
	}


	void Update(){
		SetVisibility (false);

		if (GameManager.Instance.InputController.Aim) {
			SetVisibility (true);
			Vector3 screenPosition = Camera.main.WorldToScreenPoint (transform.position);
			Reticle.transform.position = Vector3.Lerp (Reticle.transform.position, screenPosition, speed * Time.deltaTime);
		}
	}


	void SetVisibility (bool value){
		Reticle.gameObject.SetActive (value);
	}


	public void ApplyScale(float scale){
		crossTop.localPosition = new Vector3 (0, reticleStartPoint + scale, 0);
		crossBottom.localPosition = new Vector3 (0, -reticleStartPoint - scale, 0);
		crossLeft.localPosition = new Vector3 (-reticleStartPoint - scale, 0, 0);
		crossRight.localPosition = new Vector3 (reticleStartPoint + scale, 0, 0);
	}


}