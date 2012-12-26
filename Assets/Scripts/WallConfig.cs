using UnityEngine;
using System.Collections;

public class WallConfig : MonoBehaviour {
	
	public Transform[] wallTypes;
	int currentType = 0;

	
	void Start () {
	}
	
	void Update () {

	}
	

	public void setWallType(int type) {
		currentType = type;
		for (int i = 0 ; i < wallTypes.Length; i++) {
			wallTypes[i].renderer.enabled = false;
			BoxCollider[] allColliders = GetComponentsInChildren<BoxCollider>();
			foreach (BoxCollider collider in allColliders) collider.enabled = false;
		}
		wallTypes[currentType].renderer.enabled = true;
		BoxCollider[] usedColliders = wallTypes[currentType].GetComponentsInChildren<BoxCollider>();
		foreach (BoxCollider collider in usedColliders) collider.enabled = true;
	}
	
	public int getWallType() {
		return currentType;
	}
	
	public bool isWallSet() {
		if (currentType < 0) return false;
		else return true;
	}
}
