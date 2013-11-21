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
	}
	
	void Update () {
	
	}
	
	
	public void ItemTapped(string itemName) {
		Application.LoadLevel("MapScreen");
	}
}
