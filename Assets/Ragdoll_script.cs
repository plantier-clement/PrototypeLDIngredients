using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll_script : Destructible {

	private Rigidbody[] bodyParts;
	private MoveController moveController;
	public Animator animator;

	[SerializeField] SpawnPoint[] spawnPoints;


	void SpawnAtNewSpawnpoint(){
		int spawnIndex = Random.Range (0, spawnPoints.Length);
		transform.position = spawnPoints [spawnIndex].transform.position;
		transform.rotation = spawnPoints [spawnIndex].transform.rotation;
	}

	public override void Die ()
	{
		base.Die ();
		EnableRagdoll (true);
		animator.enabled = false;

		GameManager.Instance.Timer.Add (() => {
			EnableRagdoll (false);
			SpawnAtNewSpawnpoint ();
			animator.enabled = true;
			Reset ();
		}, 5);
	}

	void EnableRagdoll(bool value){
		for (int i = 0; i < bodyParts.Length; i++) {
			bodyParts [i].isKinematic = !value;
		}
	
	}

	void Start(){

		bodyParts = transform.GetComponentsInChildren<Rigidbody> ();
		EnableRagdoll (false);
		moveController = GetComponent <MoveController> ();
	}

	void Update(){
		if (!IsAlive)
			return;
		
		animator.SetFloat ("Vertical", 1);
		moveController.Move (new Vector2 (5,0));
	}



}
