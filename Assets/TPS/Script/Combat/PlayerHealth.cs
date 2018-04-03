using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Destructible {

	[SerializeField] 
	Ragdoll ragdoll;

	[SerializeField] 
	SpawnPoint[] spawnPoints;

	public float respawnWaitingTime = 2.0f;

	void SpawnAtNewSpawnpoint(){
		int spawnIndex = Random.Range (0, spawnPoints.Length);
		transform.position = spawnPoints [spawnIndex].transform.position;
		transform.rotation = spawnPoints [spawnIndex].transform.rotation;
	}


	public override void Die ()
	{
		base.Die ();
		ragdoll.EnableRagdoll (true);

		GameManager.Instance.Timer.Add (SpawnAtNewSpawnpoint, respawnWaitingTime);

	}


	[ContextMenu("Test Die!")]
	void TestDie(){
		Die ();

	}

	
}
