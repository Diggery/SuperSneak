using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerControl : MonoBehaviour {
	
	bool hackinging;
	bool hacked;
	PlayerController playerController;

	Transform miniMapDot;
	Transform miniMapDotHacked;
	
	GameControl gameControl;

	void Start () {

		
		// switch the chests minimap dot
		miniMapDot = transform.Find("MiniMap");
		miniMapDotHacked = transform.Find("miniMapDotHacked");
		
		GameObject gameControlObj = GameObject.Find ("GameControl");
		gameControl = gameControlObj.GetComponent<GameControl>();

	}
	
	
	IEnumerator HackServer() {
		
	//	animation.Play("HackServer");
		Events.Send(gameObject, "ServerStatus", "Hacking");

		
		yield return new WaitForSeconds(1);//animation["HackServer"].length);
		
	//	miniMapDot.renderer.enabled = false;
	//	miniMapDotHacked.renderer.enabled = true;
		gameControl.ServerHacked();
		Hacked();

	}
	
	public void Hacked() {
		if (hacked) return;
		hacked = true;
		print (transform.name + " is hacked");
	}
	
    void OnTriggerEnter(Collider other) {
		if (!hacked && other.transform.tag.Equals("Player")) {
			playerController = other.transform.GetComponent<PlayerController>();
			if (!playerController) Debug.Log("ERROR: crate cant find player");
			playerController.HackServer(transform.position);
			StartCoroutine(HackServer());
		}
	}	
}
