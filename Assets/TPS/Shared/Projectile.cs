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

	void OnTriggerEnter(Collider other){
		var destructible = other.transform.GetComponent<Destructible> ();
		if (destructible == null)
			return;

		destructible.TakeDamage (damage);

	}

	void Update (){
		transform.Translate (Vector3.forward * speed * Time.deltaTime);

	}

}
