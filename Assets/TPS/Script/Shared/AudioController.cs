using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour {

	AudioSource source;
	bool canPlay;


	void Start () {
		source = GetComponent<AudioSource>();
		canPlay = true;
	}


	public void Play(AudioClip clip){

		if (!canPlay)
			return;

		canPlay = false;
		source.PlayOneShot (clip);
		canPlay = true;
	}

}
