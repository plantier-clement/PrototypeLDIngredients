using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

	[System.Serializable]
	public class CameraRig{
		public Vector3 CameraOffset; 
		public float Damping;		
		public float CrouchHeight;
	}

	[SerializeField] 
	CameraRig defaultCamera;
	[SerializeField] 
	CameraRig aimCamera;

	[Header ("Camera Wall Collision")]
	public float CamCollOffsetX = 0.2f;
	public float CamCollOffsetZ = 0.2f;


	Transform cameraLookTarget;
	Player localPlayer;


	void Awake () {
		GameManager.Instance.OnLocalPlayerJoined += HandleOnLocalPlayerJoined;
	}


	void LateUpdate (){
		if (localPlayer == null)
			return;

		CameraRig cameraRig = defaultCamera;

		if (localPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMING || 
			localPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMEDFIRING)
			cameraRig = aimCamera;


		float targetHeight = cameraRig.CameraOffset.y + (localPlayer.PlayerState.MoveState == PlayerState.EMoveState.CROUCHING ? cameraRig.CrouchHeight : 0);

		Vector3 targetPosition = cameraLookTarget.position + localPlayer.transform.forward * cameraRig.CameraOffset.z +
									localPlayer.transform.up * targetHeight + 
									localPlayer.transform.right * cameraRig.CameraOffset.x;

		// Quaternion targetRotation = Quaternion.LookRotation (cameraLookTarget.position - targetPosition, Vector3.forward); // camera is facing the same direction as target but can rotate if too close
		// Quaternion targetRotation = localPlayer.transform.localRotation; // camera is always facing in the same direction as player

	
		Vector3 collisionDestination = cameraLookTarget.position + localPlayer.transform.up * targetHeight - localPlayer.transform.forward * .5f;
		Debug.DrawLine (targetPosition, collisionDestination, Color.blue);

		HandleCameraCollision (collisionDestination, ref targetPosition);

		Quaternion targetRotation = cameraLookTarget.rotation; 
		transform.position = Vector3.Lerp (transform.position, targetPosition, cameraRig.Damping * Time.deltaTime);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, cameraRig.Damping * Time.deltaTime);
	}


	private void HandleCameraCollision(Vector3 toTarget, ref Vector3 fromTarget){
		RaycastHit hit;
		if (Physics.Linecast (toTarget, fromTarget, out hit)) {
			Vector3 hitPoint = new Vector3 (hit.point.x + hit.normal.x * CamCollOffsetX, hit.point.y, hit.point.z + hit.normal.z * CamCollOffsetZ);
			fromTarget = new Vector3 (hitPoint.x, fromTarget.y, hitPoint.z);
		}
	}


	void HandleOnLocalPlayerJoined (Player player)	{
		localPlayer = player;
		cameraLookTarget = localPlayer.transform.Find ("AimingPivot");

		if (cameraLookTarget == null)
			cameraLookTarget = localPlayer.transform;
	}
}

