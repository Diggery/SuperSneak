using UnityEngine;
using System.Collections;

public class LevelEntrance : MonoBehaviour {
	
	public GameObject playerPrefab;
	public GameObject cameraPrefab;
	public GameObject miniMapPrefab;
	public Transform entrance;
	FadeTrigger entranceRoof;
	GameControl gameControl;


	void Start () {
		GameObject player = Instantiate(playerPrefab, entrance.position, entrance.rotation) as GameObject;	
		player.name = "Player";
		
		Transform entranceTop = transform.Find("EntranceTop");
		Transform closeEntrance = transform.Find("CloseEntrance");
		
		entranceRoof = entranceTop.gameObject.AddComponent<FadeTrigger>();
		TriggerRelay closeTrigger = closeEntrance.gameObject.AddComponent<TriggerRelay>();
		closeTrigger.target = entranceRoof;
		closeTrigger.setTo = false;
		
		GameObject gameControlObj = GameObject.Find ("GameControl");
		gameControl = gameControlObj.GetComponent<GameControl>();		
		
		Invoke("CreateCamera", 1);
	}
	
	void CreateCamera() {
		GameObject camera = Instantiate(cameraPrefab, entrance.position, entrance.rotation) as GameObject;
		Instantiate(miniMapPrefab, Vector3.zero, Quaternion.identity);
		
		camera.GetComponent<CameraControl>().SetUp();
		camera.AddComponent<TouchInterface>();
		camera.AddComponent<TouchManager>();
		camera.AddComponent<KeyboardControl>();
	}
	
	void Update() {
		if (!entranceRoof.triggered && gameControl.LevelComplete()) {
			print ("Exiting Level");
		}
	}
}
