using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}
	
	
	public void ItemTapped(string itemName) {
		if (itemName == "LoadLevel") Application.LoadLevel("MainScene");
	}
}
