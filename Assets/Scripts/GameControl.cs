using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	
	LevelController levelController; 
	
	int cratesOpened;
	public bool allCratesOpened;
	public bool serverHacked;
	
	public int gameMapSeed = 1;
	int currentSeed;
	string levelToLoad;
	
	public string currentLevel;
	public bool currentLevelPassed;

	
	public GameObject dialogBoxPrefab;
	DialogControl dialogBox;
	
	void Awake () {
    	DontDestroyOnLoad (transform.gameObject);
		Events.Listen(gameObject, "KeysPressed"); 
	}

	public void CrateOpened() {
		print("CrateOpened");
		
		cratesOpened++;
		if (!levelController) levelController = LevelController.instance;
		if (!allCratesOpened && levelController.getNumberOfCrates() <= cratesOpened) {
			print("All Crates Gone");
			allCratesOpened = true;
		}		
	}	
	
	public bool IsLevelComplete() {
		return serverHacked;
	}
	
	public void ServerHacked() {
		print("SERVER IS HACKED");
		serverHacked = true;

		GameObject[] servers = GameObject.FindGameObjectsWithTag("Server");
		
		foreach (GameObject server in servers) {
			server.GetComponent<ServerControl>().Hacked();
		}
	}	
		
	public void PlayerIsDead() {
		print("Game Over");
		
		GameObject guardRoom = GameObject.FindGameObjectWithTag("GuardRoom");
		guardRoom.GetComponent<GuardRoom>().ShutDown();
		
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		
		GameObject killer = FindClosestEnemy(player.transform.position);
				
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in allEnemies) {
			if (enemy == killer) {
				enemy.GetComponent<EnemyController>().LookAtDeadPlayer();
			} else {
				EnemyController controller = enemy.GetComponent<EnemyController>();
				controller.StandDown();
			}
		}
		currentLevelPassed = false;
		Invoke ("ReturnToMap", 5);
	}
	
	void Update() {
		
	}
	
	public void ReturnToMap() {
		LoadNewLevel("MapScreen", 1);
		
		ShowDialogText("Opening Map Screen...", 3, 0.25f, false);
	}
	
	public int GetGameMapSeed() {
		return gameMapSeed;	
	}	
	
	public int GetSeed() {
		return currentSeed;	
	}
	public void SetSeed(int newSeed) {
		currentSeed = newSeed;
		print ("currentSeed is set to " + currentSeed);
	}
		
	public void KeysPressed(Events.Notification notification) {
		string key = (string)notification.data;
		if (key == "Back") LoadNewLevel("MainMenu", 1);

		ShowDialogText("Returning to Menu...", 3, 0.25f, false);

	}
	
	public void LaunchLevelFromSeed(int newSeed) {
		SetSeed(Mathf.Abs(newSeed));
		LoadNewLevel("GameLevel", 1);
		ShowDialogText("Traveling to Location...", 3, 0.25f, false);
	}
	
	public void LaunchLevelFromMap(Transform selectedDot) {
		
		currentLevel = selectedDot.name;
		currentLevelPassed = false;
		float seed = (selectedDot.position.x * 1000) + (selectedDot.position.z * 100) ;
		int seedAsInt = Mathf.CeilToInt(seed);
		LaunchLevelFromSeed(seedAsInt);
	}
	
	public void LeaveLevel() {
		allCratesOpened = false;
		currentLevelPassed = IsLevelComplete();
		ReturnToMap();
	}
	
    GameObject FindClosestEnemy(Vector3 point) {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in gos) {
            Vector3 diff = go.transform.position - point;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
	
	public void ShowDialogText(string text, float delay, float scale, bool tappable) {
		if (!dialogBox) {
			GameObject dialogBoxObj = Instantiate(dialogBoxPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			dialogBox = dialogBoxObj.GetComponent<DialogControl>();
			dialogBox.transform.parent = Camera.main.transform;
			dialogBox.Init();
		}
		dialogBox.SetText(text, delay, scale, tappable);
	}
	
	public void LoadNewLevel(string levelname, float delay) {
		levelToLoad = levelname;
		Invoke("DoLoad", delay);	
	}
	
	public void DoLoad() {
		if (levelToLoad.Equals("empty")) return;
		Application.LoadLevel(levelToLoad);	
		levelToLoad = "empty";
	}
	
	public void OnApplicationQuit() {
		GameLoadSave.SaveAll();
	}
}
