using UnityEngine;
using System;
using System.Collections;

public class MenuControl : MonoBehaviour {
	
	public GameObject gameControlPrefab;
	GameControl gameControl;

	void Start () {
		GameObject gameControlObj = GameObject.Find ("GameControl");
		if (!gameControlObj) gameControlObj = Instantiate(gameControlPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		gameControlObj.name = "GameControl";
		
		gameControl = gameControlObj.GetComponent<GameControl>();
		
		TextMesh[] items = gameObject.GetComponentsInChildren<TextMesh>();
		
		foreach (TextMesh label in items) {
			label.transform.name = label.text;	
		}
	}
	
	void Update () {
	
	}
	
	
	public void ItemTapped(string itemName) {
		
		if (itemName.Equals("Continue Game")) {
			gameControl.currentLevel = "Menu";
			gameControl.LoadNewLevel("MapScreen", 1);	
			gameControl.ShowDialogText("Hold on", 3, 0.75f);
		}
		if (itemName.Equals("New Game")) {
			gameControl.ShowDialogText("Delete Game?\nAre you sure?", 100, 1.0f, transform, "NewGame");
		}
	}
	
	public void NewGameOK() {
		GameLoadSave.DeleteAll();
		gameControl.currentLevel = "Menu";
		gameControl.LoadNewLevel("MapScreen", 1);	
		gameControl.ShowDialogText("Hold on", 3, 0.75f);
	}
	
	public void NewGameCancel() {
			print ("Cancel");
		
	}
}
