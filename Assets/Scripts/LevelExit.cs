using UnityEngine;
using System.Collections;

public class LevelExit : MonoBehaviour {
	
	LevelEntrance entrance;
	
	bool leaving;

	void Start () {
		entrance = transform.parent.GetComponent<LevelEntrance>();
		leaving = false;
	}
	
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (leaving) return;
		print (other.transform.root.tag);
		if (other.transform.root.tag.Equals("Player")) {
			leaving = true;
			entrance.LeaveLevel();
		}
	}
}
