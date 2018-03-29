using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour {

	Waypoint[] waypoints;
	public float gizmosSize = 0.2f;

	int currentWaypointIndex = -1;
	public event System.Action<Waypoint> OnWaypointChanged;



	void Awake(){
		waypoints = GetWaypoint ();
	
	}


	public void SetNextWaypoint(){

		currentWaypointIndex++;

		if (currentWaypointIndex == waypoints.Length)
			currentWaypointIndex = 0;

		if (OnWaypointChanged != null)
			OnWaypointChanged (waypoints [currentWaypointIndex]);
	}

	Waypoint[] GetWaypoint(){
		return GetComponentsInChildren <Waypoint> ();
	}


	void OnDrawGizmos(){
		Gizmos.color = Color.blue;
		Vector3 previousWaypoint = Vector3.zero;

		foreach (var waypoint in GetWaypoint ()) {
			Vector3 waypointPosition = waypoint.transform.position;
			Gizmos.DrawWireSphere (waypoint.transform.position, gizmosSize);

			if (previousWaypoint != Vector3.zero)
				Gizmos.DrawLine (previousWaypoint, waypointPosition);
			previousWaypoint = waypointPosition;
		}
	}
}
