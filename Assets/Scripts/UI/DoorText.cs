using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorText : MonoBehaviour {

	public static int text;

	string message;
	Text PopupText;

	// Use this for initialization
	void Awake () {
		PopupText = GetComponent <Text> ();
	}


	public void SetText (string message)
	{
		PopupText.text = message;
		Debug.Log ("message sent");
	}

	// Update is called once per frame
	void Update () {
		
	}
}