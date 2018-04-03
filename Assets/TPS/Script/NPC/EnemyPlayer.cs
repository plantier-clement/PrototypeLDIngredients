using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyHealth))]

public class EnemyPlayer : MonoBehaviour {

	[SerializeField] 
	Scanner playerScanner;

	[SerializeField] 
	SwatSoldier settings;

	PathFinder pathFinder;

	Player priorityTarget;
	List<Player> myTargets;

	public event System.Action<Player> OnTargetSelected;



	private EnemyHealth m_EnemyHealth;
	public EnemyHealth EnemyHealth{
		get{
			if (m_EnemyHealth == null)
				m_EnemyHealth = GetComponent <EnemyHealth> ();
			return m_EnemyHealth;
		}

	}

	void Start () {
		pathFinder = GetComponent <PathFinder> ();
		pathFinder.Agent.speed = settings.WalkSpeed;

		playerScanner.OnScanReady += Scanner_OnScanReady;
		Scanner_OnScanReady ();

		EnemyHealth.OnDeath += EnemyHealth_OnDeath;
	}

	void EnemyHealth_OnDeath ()
	{
		
	}

	void Scanner_OnScanReady () {

		if (priorityTarget != null)
			return;

		myTargets = playerScanner.ScanForTargets<Player> ();

		if (myTargets.Count == 1)
			priorityTarget = myTargets [0];
		else
			SelectClosestTarget (myTargets);

		if (priorityTarget != null) {

			if (OnTargetSelected != null) {
				OnTargetSelected (priorityTarget);	
			}
		}
	}
		


	private void SelectClosestTarget(List<Player> targets){
		float closestTarget = playerScanner.ScanRange;

		foreach (var possibleTargets in myTargets) {
			if (Vector3.Distance (transform.position, possibleTargets.transform.position) < closestTarget)
				priorityTarget = possibleTargets;
		}
	}

	void Update(){
		if (priorityTarget == null)
			return;

		transform.LookAt (priorityTarget.transform.transform.position);
	}

}
