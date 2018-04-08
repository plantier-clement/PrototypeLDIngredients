using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

[RequireComponent(typeof(Player))]
public class PlayerNetwork : NetworkBehaviour {


	Player player;
	PlayerMove playerMove;
	PlayerAnimation playerAnimation;
	NetworkState state;
	NetworkState lastSentState;
	NetworkState lastReceivedState;
	NetworkState lastSentRpcState;

	List<NetworkState> predictedStates;


	[System.Serializable]
	public partial class NetworkState : InputController.InputState
	{
		public float PositionX;
		public float PositionY;
		public float PositionZ;
		public float RotationAngleY;
		public float TimeStamp;
		public float AimTargetX;
		public float AimTargetY;
		public float AimTargetZ;
	}



	void Start () {
		player = GetComponent <Player> ();
		playerMove = GetComponent <PlayerMove> ();
		playerAnimation = GetComponent <PlayerAnimation> ();
		state = new NetworkState ();
		predictedStates = new List<NetworkState> ();

		if(isLocalPlayer)
			player.SetAsLocalPlayer ();
	}


	void Update() {

		if (isLocalPlayer) {

			state = CollectInput ();
			playerMove.SetInputController (state);
			playerMove.Move (state.Horizontal, state.Vertical);
		}

		if (lastReceivedState == null)
			return;

		UpdateState ();
	
	}


	void FixedUpdate(){
	
		if (isLocalPlayer) {
			
			if (isInputStateDirty (state, lastSentState)) {
			
				lastSentState = state;
				Cmd_HandleInput (SerializeState(lastSentState));

				state.PositionX = transform.position.x;
				state.PositionY = transform.position.y;
				state.PositionZ = transform.position.z;

				predictedStates.Add (state);
			}
		}

		if (isServer && lastReceivedState != null) {
		
			NetworkState stateSolution = new NetworkState {
			
				PositionX = transform.position.x,
				PositionY = transform.position.y,
				PositionZ = transform.position.z,
				Horizontal = lastReceivedState.Horizontal,
				Vertical = lastReceivedState.Vertical,
				AimAngle = lastReceivedState.AimAngle,
				CoverToggle = lastReceivedState.CoverToggle,
				Fire1 = lastReceivedState.Fire1,
				Aim = lastReceivedState.Aim,
				IsAiming = lastReceivedState.IsAiming,
				IsCrouched = lastReceivedState.IsCrouched,
				IsInCover = lastReceivedState.IsInCover,
				IsSprinting = lastReceivedState.IsSprinting,
				IsWalking = lastReceivedState.IsWalking,
				Reload = lastReceivedState.Reload,
				RotationAngleY = lastReceivedState.RotationAngleY,
				TimeStamp = lastReceivedState.TimeStamp
			};

			if(isInputStateDirty (stateSolution, lastSentRpcState)) {
			
				lastSentRpcState = stateSolution;
				Rpc_HandleStateSolution (SerializeState (lastSentRpcState));
			}
				
		}
			
	}

	
	[Command]
	void Cmd_HandleInput (byte[] data) {

		lastReceivedState = DeserializeState (data);
		
	}

	[ClientRpc]
	void Rpc_HandleStateSolution (byte[] data) {

		lastReceivedState = DeserializeState (data);
	}


	private BinaryFormatter bf = new BinaryFormatter();

	private byte[]SerializeState (NetworkState state){
		using (MemoryStream stream = new MemoryStream ()) {
		
			bf.Serialize (stream, state);
			return stream.ToArray ();
		}
	}

	private NetworkState DeserializeState(byte[] bytes){
		using (MemoryStream stream = new MemoryStream ()) {

			return (NetworkState)bf.Deserialize (stream);
		}
	
	}


	bool isInputStateDirty(NetworkState a, NetworkState b){

		if (b == null)
			return true;
	
		return 	a.AimAngle != 		b.AimAngle ||
				a.CoverToggle != 	b.CoverToggle ||
				a.Fire1 != 			b.Fire1 ||
				a.Aim != 			b.Aim ||
				a.Horizontal != 	b.Horizontal ||
				a.Vertical != 		b.Vertical ||
				a.IsAiming != 		b.IsAiming ||
				a.IsCrouched !=		b.IsCrouched ||
				a.IsSprinting !=	b.IsSprinting ||
				a.IsWalking != 		b.IsWalking ||
				a.Reload != 		b.Reload ||
				a.RotationAngleY != b.RotationAngleY;

		}

	void UpdateState(){

		Vector3 serverPosition = new Vector3 (lastReceivedState.PositionX, lastReceivedState.PositionY, lastReceivedState.PositionZ);

		if(isLocalPlayer && !isServer) {

			// remove old states if any
			predictedStates.RemoveAll (x => x.TimeStamp < lastReceivedState.TimeStamp);

			//we expect the state to be there
			var predictedState = predictedStates.Where (x => x.TimeStamp < lastReceivedState.TimeStamp).First ();

			Vector3 predictedPosition = new Vector3 (predictedState.PositionX, predictedState.PositionY, predictedState.PositionZ);
			float positionDifferenceFromServer = Vector3.Distance (predictedPosition, serverPosition);

			if (positionDifferenceFromServer > .3f)
				transform.position = Vector3.Lerp (transform.position, serverPosition, player.Settings.RunSpeed * Time.deltaTime);

 		}


		if (!isLocalPlayer) {

			playerAnimation.Vertical = lastReceivedState.Vertical;
			playerAnimation.Horizontal = lastReceivedState.Horizontal;
			playerAnimation.IsWalking = lastReceivedState.IsWalking;
			playerAnimation.IsSprinting = lastReceivedState.IsSprinting;	
			playerAnimation.IsCrouched = lastReceivedState.IsCrouched;
			playerAnimation.IsAiming = lastReceivedState.IsAiming;
			playerAnimation.IsInCover = lastReceivedState.IsInCover;
			playerAnimation.AimAngle = lastReceivedState.AimAngle;

			Vector3 shootingSolution = new Vector3 (lastReceivedState.AimTargetX, lastReceivedState.AimTargetY, lastReceivedState.AimTargetZ);
			playerMove.SetInputController (lastReceivedState);
			player.SetInputState (lastReceivedState);

			if (shootingSolution != Vector3.zero)
				player.WeaponController.ActiveWeapon.SetAimPoint (shootingSolution);


			transform.rotation = Quaternion.Euler (transform.rotation.eulerAngles.x, lastReceivedState.RotationAngleY, transform.rotation.eulerAngles.z);
			playerMove.Move (lastReceivedState.Horizontal, lastReceivedState.Vertical);

			if (!isServer) {
				float positionDifferenceFromServer = Vector3.Distance (transform.position, serverPosition);
				if (positionDifferenceFromServer > 0.3f)
					transform.position = Vector3.Lerp (transform.position, serverPosition, player.Settings.RunSpeed * Time.deltaTime);
			
			}

		}
	}


	private NetworkState CollectInput(){

		var state = new NetworkState {
			CoverToggle = GameManager.Instance.InputController.CoverToggle,
			Fire1 = GameManager.Instance.InputController.Fire1,
			Aim = GameManager.Instance.InputController.Aim,
			Horizontal = GameManager.Instance.InputController.Horizontal,
			Vertical = GameManager.Instance.InputController.Vertical,
			IsCrouched = GameManager.Instance.InputController.IsCrouched,
			IsSprinting = GameManager.Instance.InputController.IsSprinting,
			IsWalking = GameManager.Instance.InputController.IsWalking,
			IsInCover =  GameManager.Instance.InputController.IsInCover,
			Reload = GameManager.Instance.InputController.Reload,
			RotationAngleY = transform.rotation.eulerAngles.y,
			TimeStamp = Time.time

		};

		if (state.Fire1) {
			Vector3 shootingSolution = player.WeaponController.GetImpactPoint ();
			state.AimTargetX = shootingSolution.x;
			state.AimTargetY = shootingSolution.y;
			state.AimTargetZ = shootingSolution.z;
		
		
		}
		return state;
	}

}
