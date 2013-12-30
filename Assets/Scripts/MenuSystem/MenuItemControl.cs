using UnityEngine;
using System.Collections;

public class MenuItemControl : MonoBehaviour {
	
	MenuControl menuControl;

	void Start () {
		menuControl = transform.root.GetComponent<MenuControl>();
	}
	
	void Update () {
	
	}
	
	public void tap(TouchManager.TapEvent touchEvent) {
		menuControl.ItemTapped(transform.name);
	}
	
}