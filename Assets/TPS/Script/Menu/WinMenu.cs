using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class WinMenu : MonoBehaviour {


	[SerializeField]
	GameObject WinMenuPanel;
	[SerializeField]
	Button BackToMenuButton;


	void Start () {

		WinMenuPanel.SetActive (false);
		GameManager.Instance.EventBus.AddListener ("OnAllEnemiesKilled", () =>
			{
				GameManager.Instance.Timer.Add (() =>
					{
						GameManager.Instance.IsPaused = true;
						WinMenuPanel.SetActive(true);
						GameManager.Instance.LocalPlayer.SetCursor (false);
					}, 4);
			});
		
		BackToMenuButton.onClick.AddListener (() => 
			{
				SceneManager.LoadScene ("MainMenu");
			}); 
	}


	void Update () {
		
	}



}
