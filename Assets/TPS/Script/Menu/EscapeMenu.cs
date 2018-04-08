using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour {


	[SerializeField]
	GameObject EscapeMenuPanel;
	[SerializeField]
	Button YesButton;
	[SerializeField]
	Button NoButton;



	void Awake(){
		EscapeMenuPanel.SetActive (false);
		YesButton.onClick.AddListener (OnYesClicked);
		NoButton.onClick.AddListener (OnNoClicked);
	}


	void OnYesClicked(){
		SceneManager.LoadScene ("MainMenu");
	
	}


	void OnNoClicked(){
		EscapeMenuPanel.SetActive (false);
		GameManager.Instance.IsPaused = false;
		GameManager.Instance.LocalPlayer.SetCursor (true);
	}


	void Update(){

		if (EscapeMenuPanel.activeSelf)
			return;

		if (GameManager.Instance.InputController.Escape) {
			GameManager.Instance.IsPaused = true;
			EscapeMenuPanel.SetActive(true);
			GameManager.Instance.LocalPlayer.SetCursor (false);
		}
	
	}
}
