using UnityEngine;
using System.Collections;

public class LevelEntrance : MonoBehaviour {
	
	public GameObject playerPrefab;
	public GameObject cameraPrefab;
	public Transform entrance;

	void Start () {
		GameObject player = Instantiate(playerPrefab, entrance.position, entrance.rotation) as GameObject;	
		player.name = "Player";
		
		Invoke("CreateCamera", 1);
	}
	
	void CreateCamera() {
		GameObject camera = Instantiate(cameraPrefab, entrance.position, entrance.rotation) as GameObject;
		camera.GetComponent<CameraControl>().SetUp();
		camera.AddComponent<TouchInterface>();
		camera.AddComponent<TouchManager>();
		camera.AddComponent<KeyboardControl>();
	}
}