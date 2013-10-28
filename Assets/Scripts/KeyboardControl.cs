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
		
		//build up a vector when keys are pressed
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
		
		//normalize it for the input control
		inputVector.Normalize();
		
		//half it if shift is down
		if (Input.GetKey(KeyCode.LeftShift)) {
			inputVector *= 0.5f;
		}		
		
		// if the vector is big enough, send the input, 
		if (inputVector.sqrMagnitude > 0.1) {
			usingKeys = true;
			playerController.setMoveInputOn();
			playerController.moveInput(inputVector);
		} else {
			if (usingKeys) {
				usingKeys = false;	
				playerController.setMoveInputOff();
			}

		}

	}
}
