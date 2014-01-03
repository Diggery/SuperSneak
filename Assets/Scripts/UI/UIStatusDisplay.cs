using UnityEngine;
using System.Collections;

public class UIStatusDisplay : MonoBehaviour {
	
	public TextMesh crateCount;
	public TextMesh severStatus;
	GameControl gameControl;

	public void Start () {
		Events.Listen(gameObject, "CrateOpened");
		Events.Listen(gameObject, "ServerStatus");

		crateCount.renderer.material.renderQueue = 4000;
		severStatus.renderer.material.renderQueue = 4000;
		severStatus.renderer.material.color = Color.white;
		severStatus.text = "Server: Secure";
		
		
		GameObject gameControlObj = GameObject.Find ("GameControl");
		gameControl = gameControlObj.GetComponent<GameControl>();
		Vector2 crateStatus = gameControl.GetCrateStatus();
		crateCount.text = crateStatus.x + " of " + crateStatus.y + " crates searched";
	}


	public void CrateOpened(Events.Notification notification) {
		Vector2 crateStatus = (Vector2)notification.data;
		crateCount.text = crateStatus.x + " of " + crateStatus.y + " crates searched";

	}
	
	public void ServerStatus(Events.Notification notification) {
		string serverStatus = (string)notification.data;
		switch (serverStatus) {
		case "Hacking" :
			severStatus.renderer.material.color = Color.yellow;
			severStatus.text = "Server: Accessing";
			break;
			
		case "Hacked" :
			severStatus.renderer.material.color = Color.red;
			severStatus.text = "Server: Compromised";
			break;
			
		}
	}
}
