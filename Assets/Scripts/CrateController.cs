using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrateController : MonoBehaviour {
	
	List<string> contents;
	bool opening;
	bool opened;

	void Start () {
	
	}
	
	void Update () {
	
	}
	
	void startOpening() {
		
	}
	
	void cancelOpen() {
		
	}
	
    void OnTriggerEnter(Collider other) {
		if (other.transform.tag.Equals("Player")) {
			startOpening();
		}
	}	
	
	void OnTriggerExit(Collider other) {
		if (other.transform.tag.Equals("Player")) {
			cancelOpen();
		}		
	}
}
