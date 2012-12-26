using UnityEngine;
using System.Collections;

public class KeyboardControl : MonoBehaviour {
		
	bool usingKeys;
	PlayerController playerController;

	void Start () {
		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if (playerObj) playerController = playerObj.GetComponent<PlayerController>();
			
	}
	
	void Update () {
		Vector3 inputVector = Vector3.zero;
		if (Input.GetKey(KeyCode.W)) {
			inputVector += new Vector3(0, 1, 0);
		}
		if (Input.GetKey(KeyCode.A)) {
			inputVector += new Vector3(1, 0, 0);
		}
		if (Input.GetKey(KeyCode.S)) {
			inputVector += new Vector3(0, -1, 0);
		}
		if (Input.GetKey(KeyCode.D)) {
			inputVector += new Vector3(-1, 0, 0);
		}
		
		if (inputVector.sqrMagnitude > 0.1) {
			usingKeys = true;
			playerController.setInputOn();
			playerController.moveInput(inputVector.normalized);
		} else {
			if (usingKeys) {
				usingKeys = false;	
				playerController.setInputOff();
			}

		}

	}
}
