using UnityEngine;
using System;
using System.Collections;

public class MenuControl : MonoBehaviour {
	
	public GameObject gameControlPrefab;
	GameControl gameControl;
	
	Transform newButton;
	Transform continueButton;
	Transform optionsButton;
	Transform creditsButton;

	void Start () {
		GameObject gameControlObj = GameObject.Find ("GameControl");
		if (!gameControlObj) gameControlObj = Instantiate(gameControlPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		gameControlObj.name = "GameControl";
		
		gameControl = gameControlObj.GetComponent<GameControl>();
		
		newButton = transform.Find("NewGame");
		continueButton = transform.Find("ContinueGame");
		optionsButton = transform.Find("Options");
		creditsButton = transform.Find("Credits");
		
		if (GameLoadSave.GetGameMapSeed() == -1) {
			continueButton.renderer.enabled = false;
		}
	}
	
	void Update () {
	
	}
	
	
	public void ItemTapped(string itemName) {
		
		if (itemName.Equals("ContinueGame")) {
			gameControl.currentLevel = "Menu";
			gameControl.LoadNewLevel("MapScreen", 1);	
			gameControl.OpenInfoBox("Hold on", 3, 0.3f);
		}
		if (itemName.Equals("NewGame")) {
			gameControl.OpenConfirmationBox("This will overwrite\nyour current game?\nAre you sure?", 100, 0.3f, NewGame);
		}
	}
	
	public void NewGame(string result) {
		print (result);
		if (result.Equals("Ok")) {
			GameLoadSave.DeleteAll();
			Invoke("StartNewGame", 0.1f);			
		}
		if (result.Equals("Cancel")) {
			print ("Cancelling");
		}
	}
	public void StartNewGame() {
		gameControl.CreateGameMapSeed();
		gameControl.currentLevel = "Menu";
		gameControl.LoadNewLevel("MapScreen", 1);	
		gameControl.OpenInfoBox("Hold on", 3, 0.3f);		
	}

}
