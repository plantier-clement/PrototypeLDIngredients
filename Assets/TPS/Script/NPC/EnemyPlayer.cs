using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyState))]

public class EnemyPlayer : MonoBehaviour {

	[SerializeField] 
	public Scanner playerScanner;
	[SerializeField] 
	SwatSoldier settings;
	[SerializeField]
	EnemyScriptObject enemySettings;

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

	private EnemyState m_EnemyState;
	public EnemyState EnemyState{
		get{
			if (m_EnemyState == null)
				m_EnemyState = GetComponent <EnemyState> ();
			return m_EnemyState;
		}

	}


	void Start () {
		pathFinder = GetComponent <PathFinder> ();
		pathFinder.Agent.speed = settings.WalkSpeed;

		playerScanner.OnScanReady += Scanner_OnScanReady;
		Scanner_OnScanReady ();

		EnemyHealth.OnDeath += EnemyHealth_OnDeath;
		EnemyState.OnModeChanged += EnemyState_OnModeChanged;

	}


	void Update(){
		if (priorityTarget == null || !EnemyHealth.IsAlive)
			return;

		transform.LookAt (priorityTarget.transform.transform.position);
	}


	void EnemyState_OnModeChanged (EnemyState.EMode state)
	{
		pathFinder.Agent.speed = settings.WalkSpeed;

		if (state == EnemyState.EMode.AWARE)
			pathFinder.Agent.speed = settings.RunSpeed;
	}


	void CheckEaseWeapon(){
	
		// check if we can stop aiming
		if (priorityTarget != null)
			return;

		this.EnemyState.CurrentMode = EnemyState.EMode.UNAWARE;
	}

	void CheckContinuePatrol(){

		// check if we can continue our patrol
		if (priorityTarget != null)
			return;

		pathFinder.Agent.isStopped = false;
	}

	internal void ClearTargetAndScan(){
	
		priorityTarget = null;

		GameManager.Instance.Timer.Add (CheckEaseWeapon, enemySettings.stopAimingTimer);
		GameManager.Instance.Timer.Add (CheckContinuePatrol, enemySettings.resumePatrolTimer);

		Scanner_OnScanReady ();
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
				this.EnemyState.CurrentMode = EnemyState.EMode.AWARE;
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



}
