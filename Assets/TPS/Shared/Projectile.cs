using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField] float speed;
	[SerializeField] float timeToLive;
	[SerializeField] float damage;


	void Start(){
		Destroy (gameObject, timeToLive);
	}

	void Update (){
		transform.Translate (Vector3.forward * speed * Time.deltaTime);

		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, 5f)) {
			CheckDestructible (hit.transform);
		}

	}

	void CheckDestructible(Transform other){
		var destructible = other.transform.GetComponent<Destructible> ();
		if (destructible == null)
			return;

		destructible.TakeDamage (damage);
	}

}
