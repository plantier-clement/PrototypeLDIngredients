using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
public class EnemyPatrol : MonoBehaviour {

	[SerializeField]
	WaypointController waypointController;

	[SerializeField]
	float waitTimeMin;

	[SerializeField]
	float waitTimeMax;

	PathFinder pathFinder;

	void Start(){
		waypointController.SetNextWaypoint ();
	}

	void Awake(){
		pathFinder = GetComponent <PathFinder> ();
		pathFinder.OnDestinationReached += PathFinder_OnDestinationReached;
		waypointController.OnWaypointChanged += WaypointController_OnWaypointChanged;
	}

	private void WaypointController_OnWaypointChanged (Waypoint waypoint) {
		pathFinder.SetTarget (waypoint.transform.position);
	}

	private void PathFinder_OnDestinationReached () {
		// assume we are patroling
		GameManager.Instance.Timer.Add (waypointController.SetNextWaypoint, Random.Range (waitTimeMin, waitTimeMax));

	}


}
