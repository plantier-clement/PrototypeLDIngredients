using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyPlayer))]
public class EnemyAnimation : MonoBehaviour {

	[SerializeField] Animator animator;


	private bool m_IsCrouched;
	public bool IsCrouched {
		get { 
			return m_IsCrouched;
		}
		internal set{
			m_IsCrouched = true;
			GameManager.Instance.Timer.Add (CheckSafeToStandUp, 25f);
		}
	}

	Vector3 lastPosition;
	PathFinder pathFinder;
	EnemyPlayer enemyPlayer;

	void Awake(){
		pathFinder = GetComponent <PathFinder> ();
		enemyPlayer = GetComponent <EnemyPlayer> ();
	}
		

	private void Update(){
		float velocity = ((transform.position - lastPosition).magnitude) / Time.deltaTime;
		lastPosition = transform.position;
		animator.SetBool ("IsWalking", enemyPlayer.EnemyState.CurrentMode == EnemyState.EMode.UNAWARE);
		animator.SetFloat ("Vertical", velocity / pathFinder.Agent.speed);
		animator.SetBool ("IsAiming", enemyPlayer.EnemyState.CurrentMode == EnemyState.EMode.AWARE);
		animator.SetBool ("IsCrouching", IsCrouched);
	}


	void CheckSafeToStandUp(){

		bool isUnware = enemyPlayer.EnemyState.CurrentMode == EnemyState.EMode.UNAWARE;
		if (isUnware)
			IsCrouched = false;
	
	}

}
