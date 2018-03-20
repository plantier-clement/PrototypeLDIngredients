using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	Animator anim;

	public bool lockedAtStart;
	bool locked;
	bool isOpened;
	bool playerInRange;


	public GameObject OpenPanel = null;
	public DoorText DoorText; 

	// Use this for initialization
	void Awake () {
	
		locked = lockedAtStart;
		anim = GetComponent<Animator>();
		isOpened = false;
		playerInRange = false;
	}
		
	void OnTriggerEnter (Collider other)
	{
		if(other.tag == "Player")
		{
			OpenPanel.SetActive (true);
			playerInRange = true;
		}

	}

	void OnTriggerExit (Collider other)
	{
		if(other.tag == "Player")
		{
			OpenPanel.SetActive (false);
			playerInRange = false;
		}

	}

	void Interact()
	{
		if (locked) {
			DoorText.SetText ("Locked!");

		}
		else if (locked == false && isOpened) {
			anim.SetTrigger ("Close");
			isOpened = false;
		}

		else {
			anim.SetTrigger ("Open");
			isOpened = true;
		}
	}

 /*	void OnTriggerEnter (Collider other)
	{
		if(other.tag == "Player" && isOpened == false)
		{
			anim.SetTrigger("Open");
			isOpened = true;
		}
	
		if(other.tag == "Player" && isOpened == true)
		{
			anim.SetTrigger("Close");
			isOpened = false;
		}

	}
*/


	// Update is called once per frame
	void Update () {
		if(playerInRange && Input.GetKeyDown(KeyCode.E))
		{
			Interact();
		}

	}
}
