using UnityEngine;
using System.Collections;

public class WallConfig : MonoBehaviour {
	
	public Transform[] wallTypes;
	int currentType = 0;
	
	void Start() {
		if (wallTypes.Length == 0) setUp(); 
	}
	
	public void setUp () {
		float startRot = transform.eulerAngles.y;
		transform.eulerAngles = Vector3.zero;
		wallTypes = new Transform[3];		
		setUpNoWall();
		setUpOpenWall();
		setUpClosedWall();
		transform.eulerAngles = new Vector3(0.0f, startRot, 0.0f);
		setWallType(currentType);
	}
	
	void setUpNoWall() {
		wallTypes[0] = transform.Find ("NoWall");
		GameObject collision1Obj = new GameObject("Collision NoWall");
		collision1Obj.transform.parent = wallTypes[0];
		collision1Obj.transform.localPosition = Vector3.zero;
		BoxCollider collision1 = collision1Obj.AddComponent<BoxCollider>();
		collision1.center = new Vector3(4.85f, 1.0f, 4.85f);
		collision1.size = new Vector3(0.2f, 2.0f, 0.2f);

		GameObject collision2Obj = new GameObject("Collision NoWall");
		collision2Obj.transform.parent = wallTypes[0];
		collision2Obj.transform.localPosition = Vector3.zero;
		BoxCollider collision2 = collision2Obj.AddComponent<BoxCollider>();
		collision2.center = new Vector3(-4.85f, 1.0f, 4.85f);
		collision2.size = new Vector3(0.2f, 2.0f, 0.2f);	
	}
	
	void setUpOpenWall() {
		wallTypes[1] = transform.Find ("OpenWall");
		GameObject collision1Obj = new GameObject("Collision OpenWall");
		collision1Obj.transform.parent = wallTypes[1];
		collision1Obj.transform.localPosition = Vector3.zero;
		BoxCollider collision1 = collision1Obj.AddComponent<BoxCollider>();
		collision1.center = new Vector3(3.0f, 1.0f, 4.85f);
		collision1.size = new Vector3(4.0f, 2.0f, 0.2f);

		GameObject collision2Obj = new GameObject("Collision OpenWall");
		collision2Obj.transform.parent = wallTypes[1];
		collision2Obj.transform.localPosition = Vector3.zero;
		BoxCollider collision2 = collision2Obj.AddComponent<BoxCollider>();
		collision2.center = new Vector3(-3.0f, 1.0f, 4.85f);
		collision2.size = new Vector3(4.0f, 2.0f, 0.2f);	
	}
	
	void setUpClosedWall() {
		wallTypes[2] = transform.Find ("ClosedWall");
		GameObject collision1Obj = new GameObject("Collision ClosedWall");
		collision1Obj.transform.parent = wallTypes[2];
		collision1Obj.transform.localPosition = Vector3.zero;
		BoxCollider collision1 = collision1Obj.AddComponent<BoxCollider>();
		collision1.center = new Vector3(0.0f, 1.0f, 4.85f);
		collision1.size = new Vector3(10.0f, 2.0f, 0.2f);

	}
	

	public void setWallType(int type) {
		if (wallTypes.Length == 0) setUp(); 
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
		if (wallTypes.Length == 0) setUp(); 
		return currentType;
	}
	
	public bool isWallSet() {
		if (wallTypes.Length == 0) setUp(); 
		if (currentType < 0) return false;
		else return true;
	}
}
