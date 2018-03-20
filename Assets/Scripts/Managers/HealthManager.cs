using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour {

	// PUBLIC
	public float maxHealth = 100f;
	public float currentGhostDamage = 0f;
	public float currentHealth;
	public Slider healthSlider;

	// PRIVATE
//	bool isDead = false;

	// FUNCTION
	public IEnumerator DoDamageOTime (float damageAmount, float damageTotal, float periodInSeconds){
		float currentCount = 0;
		while(currentCount < damageTotal) {
			currentHealth -= damageAmount;
			UpdateUI ();
			yield return new WaitForSeconds(periodInSeconds);
			currentCount += damageAmount;
		}
	}


	public void UpdateUI(){
		healthSlider.value = currentHealth;
		Debug.Log ("UI updated");
	}

	public void DoGhostDamage(float dmg){

	}

	public float DoDamage(float dmg){
		currentHealth -= dmg;
		UpdateUI ();
		return currentHealth;
	}
		
	public float GetCurrentHealth (){
		float targetHealth;
		targetHealth = currentHealth;
		return targetHealth;
	}
		
	public void Kill (GameObject target){
	//	isDead = true; // if need later
		Destroy (target);
		if (target.tag == "Player") {
			SceneManager.LoadScene (0); //basic restart level
		}
	}

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		healthSlider.value = currentHealth;
	}
		
}