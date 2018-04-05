using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Destructible {


	[SerializeField] Ragdoll ragdoll;

	public override void Die () {
		base.Die ();
		ragdoll.EnableRagdoll (true);

		GameManager.Instance.EventBus.RaiseEvent ("EnemyDeath");
	}


}
